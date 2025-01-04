using System.Data.SQLite;
using Configs.app;

namespace Configs.util;

public class Database
{
    public static Database Instance { get; } = new Database();

    private readonly SQLiteConnection _connection = new SQLiteConnection("Data Source=database.db;Version=3;");

    public Database()
    {
        _connection.Open();

        _initializeTables();
    }

    ~Database()
    {
        _connection.Close();
    }

    private void _initializeTables()
    {
        // 开启外键约束
        _execute("PRAGMA foreign_keys = ON;");

        // 应用路径表
        _execute("""
                 create table if not exists command_paths(
                   id integer primary key autoincrement,
                   app string not null,
                   path string not null
                 );
                 """);
        // 配置文件路径表
        _execute("""
                 create table if not exists file_paths(
                   id integer primary key autoincrement,
                   app string not null,
                   file string not null,
                   path string not null
                 );
                 """);
        // 预设表
        _execute("""
                 create table if not exists presets(
                   id integer primary key autoincrement,
                   app string not null,
                   name string not null
                 );
                 """);
        // 预设属性表
        _execute("""
                 create table if not exists preset_properties(
                   id integer primary key autoincrement,
                   preset integer not null,
                   property string not null,
                   value string not null,
                   
                   foreign key(preset) references presets(id) on delete cascade
                 );
                 """);
    }

    public void SaveAppCommand(string app, string path)
    {
        _execute($"insert into command_paths(app, path) values('{app}', '{path}');");
    }

    public void SaveAppConfig(string app, string file, string path)
    {
        _execute($"insert into file_paths(app, file, path) values('{app}', '{file}', '{path}');");
    }

    public void SavePreset(string app, ref Preset preset)
    {
        var presetId = (int)_insert($"insert into presets(app, name) values('{app}', '{preset.Name}');");
        foreach (var property in preset.PropertyNames)
        {
            _execute($"insert into preset_properties(preset, property, value) values({presetId}, '{property}', '{preset.Properties[property]}');");
        }
    }

    public string? ReadAppCommand(string app)
    {
        using var q = _query($"select path from command_paths where app='{app}'");
        return q.Read() ? q.GetString(0) : null;
    }

    public string? ReadAppConfig(string app, string file)
    {
        using var q = _query($"select path from file_paths where app='{app}' and file='{file}'");
        return q.Read() ? q.GetString(0) : null;
    }

    public List<Preset> ReadPresets(string app)
    {
        var presets = new List<Preset>();
        using var presetParams = _query($"select id, name from presets where app='{app}'");
        while (presetParams.Read())
        {
            var id = presetParams.GetInt32(0);
            var name = presetParams.GetString(1);
            var preset = new Preset(name);
            using var presetProperties = _query($"select property, value from preset_properties where preset={id}");
            while (presetProperties.Read())
            {
                preset.AddProperty(presetProperties.GetString(0), presetProperties.GetString(1));
            }
            presets.Add(preset);
        }
        return presets;
    }

    private void _execute(string sql)
    {
        using var command = new SQLiteCommand(sql, _connection);
        command.ExecuteNonQuery();
    }

    private long _insert(string sql)
    {
        using var command = new SQLiteCommand(sql + " select last_insert_rowid();", _connection);
        var id = command.ExecuteScalar();
        return (long)id;
    }

    private SQLiteDataReader _query(string sql)
    {
        using var command = new SQLiteCommand(sql, _connection);
        var reader = command.ExecuteReader();
        return reader;
    }
}