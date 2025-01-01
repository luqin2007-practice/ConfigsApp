﻿using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Configs.util;

namespace Configs.app;

public partial record ConfigFile
{
    private static readonly JsonDocumentOptions _options = new JsonDocumentOptions()
    {
        CommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    private object? _file;

    public object Read()
    {
        if (_file != null)
        {
            return _file;
        }

        _file = Type.ToLower() switch
        {
            "json" => JsonNode.Parse(File.ReadAllText(SelectedPath), documentOptions: _options)!,
            _ => throw new NotSupportedException()
        };
        return _file;
    }

    public ExeResult Save()
    {
        if (_file == null)
        {
            return ExeResult.Ok();
        }

        switch (Type.ToLower())
        {
            case "json":
                File.WriteAllText(SelectedPath, ((JsonNode)_file).ToJsonString());
                return ExeResult.Ok();
        }

        return ExeResult.Error($"未知类型 {Type}");
    }
}