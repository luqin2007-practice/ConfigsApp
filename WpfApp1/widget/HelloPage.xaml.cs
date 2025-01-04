using System.Windows.Controls;
using System.Windows.Media;

using static Configs.util.FileUtils;

namespace Configs.widget
{
    /// <summary>
    /// 欢迎页面
    /// </summary>
    public partial class HelloPage : UserControl
    {
        public List<List<LabelText>> Introduction { get; } =
        [
            [
                "功能详情：",
                "    1. 集中配置各应用",
                "    2. 保存若干配置为预设，按需切换",
                "    3. 备份、恢复配置内容",
                $"该程序支持添加自定义程序，在程序 {AppPath} 子目录中创建一个新目录，并至少包含一个 {AppJson} 即可。"
            ],
            [
                $"通常，一个 {AppPath} 应当至少包含一个 name 属性和 icon 属性，表示应用名和应用图标",
                ("这两个属性不是必须的，但不包含 icon 图标会很丑", Brushes.Green),
                "还可以包含一个 hasPresets 属性。该属性默认为 true，表示是否可以有预设",
            ]
        ];

        public List<List<LabelText>> Commands { get; } =
        [
            [
                $"使用命令行进行管理，需要将相关命令放入 {CommandsJson} 文件中，主要包含 app, test 和 commands 几个属性",
                "app 表示应用程序执行命令行操作时，默认程序执行的命令行",
                ($"\t省略时，使用 {AppPath} 中的 name 属性值", Brushes.YellowGreen),
                "test 表示如何确定应用程序是否可用，即执行后面的指令后，如果不出错，则表示对应命令可用",
                ("\t使用 {0} 替代应用命令", Brushes.Coral),
                ("\t省略时，默认使用 where {0} 检查", Brushes.YellowGreen),
                "commands 包含所有可用属性及描述，是一个数组，其中每个元素可以是一个字符串或 command 对象"
            ],
            [
                ("字符串是一个 command 的属性的简写，表示除 property 外，其余使用默认值", Brushes.Green),
                "每个 command 对象都必须包含一个 property 属性，表示属性名，其余属性包括：",
                "\tname：显示的属性名，默认与 property 相同",
                "\ttype：属性类型，默认 string，详见数据类型选项卡",
                "\tdesc：属性描述，默认与 name 相同",
                "\toverride：替换默认读、写、恢复默认操作，包含 read、write、revoke 三个属性",
                ("\t\tread：读，默认为 \"{0} property属性\"，其中 {0} 将替换成 app", Brushes.YellowGreen),
                ("\t\twrite：设置，默认为 \"{0} property属性 {1}\"，其中 {0} 将替换成 app，{1} 将替换成新值", Brushes.Orange),
                ("\t\trevoke：恢复默认，默认设置为默认值", Brushes.DarkCyan),
                "default：默认值",
                "\t若不同平台数据不同，可以使用一个对象表示，支持 windows、linux、mac 三个属性",
            ]
        ];

        public List<List<LabelText>> Documents { get; } =
        [
            [
                $"使用配置文档的应用配置，需要将相关文档放在 {FilesJson} 文件中",
                $"{FilesJson} 中属性名即文件名，后可以有三种写法：",
                "\t字符串：文件默认位置，默认文件类型为文件名扩展名",
                "\t字符串数组：文件可能的位置，默认文件类型为文件名扩展名",
                "\t对象：包含 file 和 type 两个属性，file 可以是字符串或数组作为默认文件位置，type 表示文件类型",
            ],
            [
                $"之后创建 文件名{FilesDescExt} 和 文件名{FilesGroupExt} 两个文件，前者记录了属性信息，后者记录了分组信息"
            ],
            [
                $"{FilesDescExt} 文件应该与配置文件采用相同的结构，仅在末端每个属性部分扩展为一个对象，包含 name, type, desc, default 属性",
                "\thidden：隐藏，",
                "\tname：显示的属性名，默认与属性名相同",
                "\ttype：属性类型，默认 string，详见数据类型选项卡",
                "\tdesc：属性描述，默认与 name 相同",
                "default：默认值",
                "\t若不同平台数据不同，可以使用一个对象表示，支持 windows、linux、mac 三个属性",
            ],
            [
                $"{FilesGroupExt} 文件则描述了配置文件的分组名称。根据配置文件结构，每个组包含 name 和 children 两个属性"
            ],
        ];

        public Dictionary<string, List<LabelText>> Types { get; } = new Dictionary<string, List<LabelText>>()
        {
            ["intro"] =
            [
                "属性类型默认 string，支持 bool，int, directory, file, list 和其他自定义类型",
                "属性类型可以是一个字符串或对象。如果是一个字符串，则表示使用该类型的默认配置",
            ],
            ["string"] =
            [
                "string 类型为最基本的数据类型，表示一个字符串类型属性",
                ("选项：regex 接受一个正则表达式，用于校验字符串是否符合要求，默认不检查", Brushes.CornflowerBlue),
                "字符串类型属性的默认值为空字符串",
                "例如，下面表示一个只接受手机号码的字符串",
                ("""
                 {
                 	"type": {
                 	    "type": "string",
                 	    "regex": "^1\\d{10}$"
                 	}
                 }
                 """, Brushes.BlueViolet),
            ],
            ["bool"] =
            [
                "bool 类型表示一个布尔值类型。默认值为 false",
                ("选项：true 或 false 属性对应选中、未选中两种不同情况的值，默认为 true 和 false", Brushes.CornflowerBlue),
                "例如，下面表示一个布尔值属性，true 时对应字符串 yes，false 对应字符串 no",
                ("""
                 {
                 	"type": {
                 	    "type": "bool",
                 	    "true": "yes",
                 	    "false": "no"
                 	}
                 }
                 """, Brushes.BlueViolet),
            ],
            ["int"] =
            [
                "int 类型表示一个整数属性，默认值为最小值或 0",
                ("选项：max 或 min 可用于自定义最小或最大值，默认不做限制", Brushes.CornflowerBlue),
                "例如，下面表示一个整数，范围在 0-10",
                ("""
                 {
                 	"type": {
                        "type": "int",
                        "min": 0,
                        "max": 10
                    }
                 }
                 """, Brushes.BlueViolet),
            ],
            ["directory"] =
            [
                "directory 类型表示一个目录，默认值为 /",
                ("选项：exist 表示对应目录是否必须存在，默认为 false", Brushes.CornflowerBlue),
            ],
            ["file"] =
            [
                "file 类型表示一个文件，默认值为 /",
                ("选项：exist 表示对应文件是否必须存在，默认为 false", Brushes.CornflowerBlue),
                ("选项：ext 为一个字符串列表，表示允许的扩展名，默认不做限制", Brushes.CornflowerBlue),
                "例如，下面表示一个文件，要求文件必须存在，且只接收 .exe 或 .cmd 可执行文件",
                ("""
                 {
                 	"type": {
                 		"type": "file",
                 		"exist": true,
                 		"ext": ["exe", "cmd"]
                 	}
                 }
                 """, Brushes.BlueViolet),
            ],
            ["list"] =
            [
                "list 类型表示一个列表，默认为空",
                ("选项：split 表示列表的分隔符，默认为换行符 \\n", Brushes.CornflowerBlue),
                ("选项：elementType 表示列表中每一项的类型，默认为 string", Brushes.CornflowerBlue),
                "例如，下面表示一个文件列表，每个文件之间使用 ; 分割，只接收 .exe 或 .cmd 可执行文件",
                ("""
                 {
                 	"type": {
                        "type": "list",
                        "split": ";",
                        "elementType": {
                            "type": "file",
                            "ext": ["exe", "cmd"]
                        }
                    }
                 }
                 """, Brushes.BlueViolet),
            ],
            ["enum"] =
            [
                $"枚举类型可以直接在类型对象中声明，也可以在 {TypesJson} 中声明",
                "每个枚举值应当是一个字符串或对象，对象包含 name, value 和 desc 三个属性",
                "枚举值的默认值就是第一个值",
                "例1：可选 verbose，info，warning，error，critical 的枚举类型",
                ("""
                 {
                     "type": {
                         "type": "enum",
                         "values": [ "verbose", "info", "warning", "error", "critical" ],
                     }
                 }
                 """, Brushes.BlueViolet),
                $"例2：在 {TypesJson} 中声明的 InstallScope 类型枚举",
                ("""
                 {
                     "type": "InstallScope"
                 }
                 """, Brushes.BlueViolet),
                ("""
                 {
                 "InstallScope": [
                   {
                     "value": "user",
                     "name": "当前用户",
                     "desc": "默认安装到当前用户"
                   },
                   {
                     "value": "machine",
                     "name": "所有用户",
                     "desc": "默认安装到所有用户"
                   }
                 ],
                 }
                 """, Brushes.BlueViolet),
            ],
            ["array"] =
            [
                "array 类型仅用于配置文件，表示对应文件的列表类型，默认为空列表",
                ("选项：elementType 表示列表中每一项的类型，默认为 string", Brushes.CornflowerBlue),
            ]
        };

        public HelloPage()
        {
            InitializeComponent();
        }
    }
}