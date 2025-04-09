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
            read_type = Unpack_string(stream);
        if (read_type == "None")
            ret_value = null;
        else if (read_type == "Int")
            ret_value = Unpack_int(stream);
        else if (read_type == "bool")
            ret_value = (Unpack_int(stream) == 1);
        else if (read_type == "Float")
            ret_value = Unpack_double(stream);
        else if (read_type == "String")
            ret_value = Unpack_string(stream);
        else if (read_type == "DataFrame")
            ret_value = Unpack_dataframe(stream);
        else if (read_type == "datetime")
            ret_value = Unpack_datetime(stream);
        else if (read_type == "numpy.ndarray")
            ret_value = Unpack_numpy_array(stream);
        else if (read_type == "List")
            ret_value = Unpack_list_and_len(stream);
        else if (read_type == "Dict")
            ret_value = Unpack_dict(stream);
        else if (read_type == "Tuple")
            ret_value = Unpack_tuple(stream);
        else if (read_type == "Error")
            throw new InvalidOperationException(Unpack_string(stream));
        else
            throw new NotImplementedException($"Unsupported type {read_type}");
        return ret_value;
    }


    public static string Unpack_string(StreamReader stream)
    {
        var buffer = stream.ReadAsBytes(Sizes.Text);     
        var size = BitConverter.ToInt32(buffer, 0);
        byte[] s_buffer = stream.ReadAsBytes(size);
        return System.Text.Encoding.UTF8.GetString(s_buffer);
    }
     

    static int Unpack_int(StreamReader stream)
    {
        byte[] buffer = stream.ReadAsBytes(Sizes.Integer);
        var value = BitConverter.ToInt32(buffer, 0);
        return value;
    }


    static double Unpack_double(StreamReader stream)
    {
        byte[] buffer = stream.ReadAsBytes(Sizes.Double);  
        var value = BitConverter.ToDouble(buffer, 0);
        return value;
    }


    static object Unpack_tuple(StreamReader stream)
    {
        var length = Unpack_int(stream);
        List<object?> lst = [];
        for (int i = 0; i < length; i++)
        {
            lst.Add(Unpack_data(stream));
        }
        var value = Tuple.Create(lst);

        return value;
    }

    static List<object?> Unpack_list_and_len(StreamReader stream)
    {
        var length = Unpack_int(stream);
        var value = Unpack_list(stream, length);
        return value;
    }


    static Dictionary<object, object?> Unpack_dict(StreamReader stream)
    {
        var length = Unpack_int(stream);
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

    static List<object?> Unpack_list(StreamReader stream, int length)
    {
        List<object?> lst = [];
        if (length == 0)
            return lst;
        var read_type = Unpack_string(stream);
        for (int i = 0; i < length; i++)
            lst.Add(Unpack_data(stream, read_type));
        return lst;
    }


    static DataTable Unpack_dataframe(StreamReader stream)
    {
        var dt = new DataTable();
        var col_count = Unpack_int(stream);
        var row_count = Unpack_int(stream);
        List<List<object?>> data = [];
        for (int i = 0; i < col_count; i++)
        {
            var column_name = Unpack_string(stream);
            dt.Columns.Add(column_name, typeof(object));
            data.Add(Unpack_list(stream, row_count));
        }
        for (int r = 0; r < row_count; r++)
        {
            //  var row = dt.NewRow();
          
            dt.Rows.Add(data.Select(t => t[r]).ToArray());
        }
        var index = Unpack_list(stream, row_count);
        return dt;
    }
    static DateTime Unpack_datetime(StreamReader stream)
    {
        var year = Unpack_int(stream);
        var month = Unpack_int(stream);
        var day = Unpack_int(stream);
        var hour = Unpack_int(stream);
        var minute = Unpack_int(stream);
        var second = Unpack_int(stream);
        var microsecond = Unpack_int(stream);
        return new DateTime(year, month, day, hour, minute, second, microsecond);
    }

    static object?[] Unpack_numpy_array(StreamReader stream)
    {

        var shape = Unpack_list_and_len(stream);
        var lst = Unpack_list(stream, shape.Count);
        return lst.ToArray();
    }
}