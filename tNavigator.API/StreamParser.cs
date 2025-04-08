using System;
using System.Data;
using System.Reflection.PortableExecutable;

namespace tNavigator.API;

public class StreamParser
{
    public static object? unpack_data(StreamReader stream)
    {
        using var memstream = new MemoryStream();
        stream.BaseStream.CopyTo(memstream);
        return unpack_data(memstream);
    }

    public static object? unpack_data(Stream stream, string read_type = "")
    {
        object? ret_value;
        if (read_type == "")
            read_type = unpack_string(stream);
        if (read_type == "None")
            ret_value = null;
        else if (read_type == "Int")
            ret_value = unpack_int(stream);
        else if (read_type == "bool")
            ret_value = (unpack_int(stream) == 1);
        else if (read_type == "Float")
            ret_value = unpack_double(stream);
        else if (read_type == "String")
            ret_value = unpack_string(stream);
        else if (read_type == "DataFrame")
            ret_value = unpack_dataframe(stream);
        else if (read_type == "datetime")
            ret_value = unpack_datetime(stream);
        else if (read_type == "numpy.ndarray")
            ret_value = unpack_numpy_array(stream);
        else if (read_type == "List")
            ret_value = unpack_list_and_len(stream);
        else if (read_type == "Dict")
            ret_value = unpack_dict(stream);
        else if (read_type == "Tuple")
            ret_value = unpack_tuple(stream);
        else if (read_type == "Error")
            throw new InvalidOperationException(unpack_string(stream));
        else
            throw new NotImplementedException($"Unsupported type {read_type}");
        return ret_value;
    }


    public static string unpack_string(StreamReader stream)
    {
        using var memstream = new MemoryStream();
        stream.BaseStream.CopyTo(memstream);
        return unpack_string(memstream);
    }

    public static string unpack_string(Stream stream)
    {
        byte[] buffer = new byte[size_const.size_t];
        stream.Read(buffer, 0, size_const.size_t);
        var size = BitConverter.ToInt32(buffer, 0);
        byte[] s_buffer = new byte[size];
        stream.Read(s_buffer, 0, size);
        return System.Text.Encoding.UTF8.GetString(s_buffer);
    }


    internal static int unpack_int(Stream stream)
    {
        byte[] buffer = new byte[size_const.integer];
        stream.Read(buffer, 0, size_const.integer);
        var value = BitConverter.ToInt32(buffer, 0);
        return value;
    }


    static double unpack_double(Stream stream)
    {
        byte[] buffer = new byte[size_const._double];
        stream.Read(buffer, 0, size_const._double);
        var value = BitConverter.ToDouble(buffer, 0);
        return value;
    }


    static object unpack_tuple(Stream stream)
    {
        var length = unpack_int(stream);
        List<object> lst = [];
        for (int i = 0; i < length; i++)
        {
            lst.Add(unpack_data(stream));
        }
        var value = Tuple.Create(lst);

        return value;
    }

    static List<object> unpack_list_and_len(Stream stream)
    {
        var length = unpack_int(stream);
        var value = unpack_list(stream, length);
        return value;
    }


    static Dictionary<object?, object?> unpack_dict(Stream stream)
    {
        var length = unpack_int(stream);
        Dictionary<object, object?> dic = [];
        for (int i = 0; i < length; i++)
        {
            var key = unpack_data(stream);
            var value = unpack_data(stream);
            dic.Add(key, value);
        }
        return dic;
    }

    static List<object?> unpack_list(Stream stream, int length)
    {
        List<object?> lst = [];
        if (length == 0)
            return lst;
        var read_type = unpack_string(stream);
        for (int i = 0; i < length; i++)
            lst.Add(unpack_data(stream, read_type));
        return lst;
    }


    static DataTable unpack_dataframe(Stream stream)
    {
        var dt = new DataTable();
        var col_count = unpack_int(stream);
        var row_count = unpack_int(stream);
        List<List<object?>> data = [];
        for (int i = 0; i < col_count; i++)
        {
            var column_name = unpack_string(stream);
            dt.Columns.Add(column_name, typeof(object));
            data.Add(unpack_list(stream, row_count));
        }
        for (int r = 0; r < row_count; r++)
        {
            dt.Rows.Add(data.Select(t => t[r]));
        }
        var index = unpack_list(stream, row_count);
        return dt;
    }
    static DateTime unpack_datetime(Stream stream)
    {
        var year = unpack_int(stream);
        var month = unpack_int(stream);
        var day = unpack_int(stream);
        var hour = unpack_int(stream);
        var minute = unpack_int(stream);
        var second = unpack_int(stream);
        var microsecond = unpack_int(stream);
        return new DateTime(year, month, day, hour, minute, second, microsecond);
    }

    static object?[] unpack_numpy_array(Stream stream)
    {

        var shape = unpack_list_and_len(stream);
        var lst = unpack_list(stream, shape.Count);
        return lst.ToArray();
    }
}