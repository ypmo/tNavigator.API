using System;
using System.Data;
using System.Reflection.PortableExecutable;

namespace tNav.Common;

public static class StreamParser
{
    public static object? Unpack_data(StreamReader stream, string read_type = "")
    {
        object? ret_value;
        if (read_type == "")
            read_type = UnpackString(stream);
        if (read_type == "None")
            ret_value = null;
        else if (read_type == "Int")
            ret_value = UnpackInt(stream);
        else if (read_type == "bool")
            ret_value = (UnpackInt(stream) == 1);
        else if (read_type == "Float")
            ret_value = UnpackDouble(stream);
        else if (read_type == "String")
            ret_value = UnpackString(stream);
        else if (read_type == "DataFrame")
            ret_value = UnpackDataFrame(stream);
        else if (read_type == "datetime")
            ret_value = UnpackDatetime(stream);
        else if (read_type == "numpy.ndarray")
            ret_value = Unpack_numpy_array(stream);
        else if (read_type == "List")
            ret_value = UnpackListAndLen(stream);
        else if (read_type == "Dict")
            ret_value = UnpackDict(stream);
        else if (read_type == "Tuple")
            ret_value = UnpackTuple(stream);
        else if (read_type == "Error")
            throw new InvalidOperationException(UnpackString(stream));
        else
            throw new NotImplementedException($"Unsupported type {read_type}");
        return ret_value;
    }


    public static string UnpackString(StreamReader stream)
    {
        var buffer = stream.ReadAsBytes(Sizes.Text);     
        var size = BitConverter.ToInt32(buffer, 0);
        byte[] s_buffer = stream.ReadAsBytes(size);
        return System.Text.Encoding.UTF8.GetString(s_buffer);
    }
     

    static int UnpackInt(StreamReader stream)
    {
        byte[] buffer = stream.ReadAsBytes(Sizes.Integer);
        var value = BitConverter.ToInt32(buffer, 0);
        return value;
    }


    static double UnpackDouble(StreamReader stream)
    {
        byte[] buffer = stream.ReadAsBytes(Sizes.Double);  
        var value = BitConverter.ToDouble(buffer, 0);
        return value;
    }


    static object UnpackTuple(StreamReader stream)
    {
        var length = UnpackInt(stream);
        List<object?> lst = [];
        for (int i = 0; i < length; i++)
        {
            lst.Add(Unpack_data(stream));
        }
        var value = Tuple.Create(lst);

        return value;
    }

    static List<object?> UnpackListAndLen(StreamReader stream)
    {
        var length = UnpackInt(stream);
        var value = UnpackList(stream, length);
        return value;
    }


    static Dictionary<object, object?> UnpackDict(StreamReader stream)
    {
        var length = UnpackInt(stream);
        Dictionary<object, object?> dic = [];
        for (int i = 0; i < length; i++)
        {
            var key = Unpack_data(stream);
            if (key == null)
            {
                throw new InvalidOperationException("Пустой ключ" );
            }
            var value = Unpack_data(stream);
            dic.Add(key, value);
        }
        return dic;
    }

    static List<object?> UnpackList(StreamReader stream, int length)
    {
        List<object?> lst = [];
        if (length == 0)
            return lst;
        var read_type = UnpackString(stream);
        for (int i = 0; i < length; i++)
            lst.Add(Unpack_data(stream, read_type));
        return lst;
    }


    static DataTable UnpackDataFrame(StreamReader stream)
    {
        var dt = new DataTable();
        var col_count = UnpackInt(stream);
        var row_count = UnpackInt(stream);
        List<List<object?>> data = [];
        for (int i = 0; i < col_count; i++)
        {
            var column_name = UnpackString(stream);
            dt.Columns.Add(column_name, typeof(object));
            data.Add(UnpackList(stream, row_count));
        }
        for (int r = 0; r < row_count; r++)
        {
            //  var row = dt.NewRow();
          
            dt.Rows.Add(data.Select(t => t[r]).ToArray());
        }
        var index = UnpackList(stream, row_count);
        return dt;
    }
    static DateTime UnpackDatetime(StreamReader stream)
    {
        var year = UnpackInt(stream);
        var month = UnpackInt(stream);
        var day = UnpackInt(stream);
        var hour = UnpackInt(stream);
        var minute = UnpackInt(stream);
        var second = UnpackInt(stream);
        var microsecond = UnpackInt(stream);
        return new DateTime(year, month, day, hour, minute, second, microsecond);
    }

    static object?[] Unpack_numpy_array(StreamReader stream)
    {

        var shape = UnpackListAndLen(stream);
        var lst = UnpackList(stream, shape.Count);
        return lst.ToArray();
    }
}