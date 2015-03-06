var fixed2 = function (val) {
    if (!val) {
        return '';
    }
    return val.toFixed(2);
}
var fixedColorful = function (val) {
    if (val > 0) {
        return '<span style="color: #b00">' + val.toFixed(2) + '</span>';
    } else if (val < 0) {
        val = -val
        return '<span style="color: #0b0">' + val.toFixed(2) + '</span>';
    }
}

var cols = [
     { title: '行情', name: '', width: 40, align: 'center', lockWidth: true, renderer: function (val, item, items, rowIndex) { return '<div class="btnPrice"></div>'; } },
    { title: '时间', name: 'data_timestamp', width: 60, align: 'center', sortable: true, lockDisplay: true },
    {
        title: '证券信息', align: 'center', cols: [
                                      { title: '证券代码', name: 'secucode', width: 100, align: 'center', sortable: true, lockDisplay: true },
                                      { title: '证券简称', name: 'secuabbr', width: 110, align: 'center', sortable: true, lockDisplay: true }
        ]
    },
    { title: '现价', name: 'last_px', width: 60, align: 'center', sortable: true, type: 'number', renderer: fixedColorful },
    {
        title: '涨跌幅', name: 'pct_change_rate', width: 60, align: 'center', sortable: true, type: 'number', renderer: function (val) {
            if (val > 0) {
                return '<span style="color: #b00">' + val.toFixed(2) + '%' + '</span>';
            } else if (val < 0) {
                return '<span style="color: #0b0">' + val.toFixed(2) + '%' + '</span>';
            }
            return val.toFixed(2) + '%';
        }
    },
    {
        title: '涨跌', name: 'pct_change', width: 60, align: 'center', sortable: true, type: 'number', renderer: function (val) {
            if (val > 0) {
                return '<span style="color: #b00">' + val.toFixed(2) + '</span>';
            } else if (val < 0) {
                return '<span style="color: #0b0">' + val.toFixed(2) + '</span>';
            }
            return val.toFixed(2);
        }
    },
    //{
    //    title: '振幅', name: 'AMPLITUDE', width: 60, align: 'center', sortable: true, type: 'number', renderer: function (val) {
    //        return val.toFixed(2) + '%';
    //    }
    //},
    {
        title: '成交', align: 'center', cols: [
                                  {
                                      title: '成交量(手)', name: 'business_amount', width: 80, align: 'center', type: 'number', sortable: true, renderer: function (val) {
                                          return (val / 100).toFixed(2);
                                      }
                                  },
                                  {
                                      title: '成交额(万)', name: 'business_balance', width: 80, align: 'center', type: 'number', sortable: true, renderer: function (val) {
                                          return (val / 10000).toFixed(2);
                                      }
                                  }
        ]
    },
    { title: '昨收盘', name: 'preclose_px', width: 60, align: 'center', sortable: true, type: 'number', renderer: fixed2 },
    { title: '今开盘', name: 'open_px', width: 60, align: 'center', sortable: true, type: 'number', renderer: fixedColorful },
    { title: '最高价', name: 'high_px', width: 60, align: 'center', hidden: false, sortable: true, type: 'number', renderer: fixedColorful },
    { title: '最低价', name: 'low_px', width: 60, align: 'center', hidden: false, sortable: true, type: 'number', renderer: fixedColorful }
];
var mmg = null;
var pg = null;

$(document).ready(function () {
    mmg = $('#mmg').mmGrid({
        //width: auto,                  //表格宽度。参数为'auto'和以'%'结尾,表格可在调整窗口尺寸时根据父元素调整尺寸。
        height: 560,                  //表格高度。如果以'%'结尾,表格可在调整窗口尺寸时根据父元素调整尺寸。如果设置为'auto'，表格显示全部行，不显示垂直滚动条
        url: 'Home/InitializePaginationInfo',        //AJAX请求数据的地址。
        //params:object, function(data){ return {}; },  //AJAX请求的参数。可以是普通对象或函数。 函数返回一个参数对象，每次调用AJAX时调用。如果返回为空则不会调用AJAX。
        method: 'get',             //AJAX提交方式。
        //cache:true,               //AJAX缓存。
        //items: items,             //数据使用本地对象数组。
        root: 'items',                //数据使用本地对象数组。
        nowrap: true,               //表格显示的数据超出列宽时是否换行。
        multiSelect: false,         //行多选。
        fullWidthRows: true,    //true:表格第一次加载数据时列伸展，自动充满表格。
        checkCol: false,            //表格显示checkbox
        indexCol: true,             //表格显示索引列
        indexColWidth: 30,      //表格索引列宽度
        loadingText: '正在载入......',//数据载入时的提示文字。
        noDataText: '没有数据',  //没有数据时的提示文字。
        loadErrorText: '数据加载出现异常',//护具加载异常的提示文字。
        cols: cols,                     //数据模型。
        sortName: 'secucode', //排序的字段名。字段名的值为列模型设置的sortName或name中的值。
        sortStatus: 'asc',          //排序方向。
        //remoteSort:false,     //是否使用服务器端排序。当值为true时，sortName和sortStatus会作为参数放入AJAX请求中。
        autoLoad: false,          //是否表格准备好时加载数据。
        showBackboard: true, //是否显示选项背板。
        plugins: [
            pg = $('#pg').mmPaginator()
        ] //分页插件
    });
    $("#main_tree span").each(function (i) {
        if ($(this).hasClass("active")) {
            InitializeRealInfo($(this).attr('id'));
        }
    });
    timename = setInterval("RefreshBaseData();", 10000);
});


function RefreshBaseData() {
    $.ajax({
        success: function (data) {
            mmg.load();
        }
    })
}

function ChangeBaseData(id) {
    pg.$el.data('page', 1)
    InitializeStyle(id);
    InitializeRealInfo(id);
}

function InitializeRealInfo(id) {
    $.ajax({
        url: 'Home/InitializeBaseRealInfo',
        data: "id=" + id,
        async: true,
        success: function (data) {
            mmg.load();
        }
    });
}

function InitializeStyle(id) {
    $("#main_tree span").each(function (i) {
        if ($(this).hasClass("active"))
            $(this).removeClass("active");
    });

    $("#main_tree").find("span").each(function (i) {
        if ($(this).attr('id') == id)
            $(this).addClass("active");
    });
}

$(function () {
    $('#main_tree dt').click(function () {
        $current_dt = $(this);
        $('dt').each(function (i) {
            if ($(this).text() == $current_dt.text()) {
                $(this).next('dd').slideToggle('slow');
                $(this).toggleClass('node');
            }
            else {
                var style = $(this).next('dd').attr('style');
                if (style != null && style.indexOf("block", 0) > 0)
                    $(this).toggleClass('node');
                $(this).next('dd').slideUp('slow');
            }
        });
    });
})

function Search(searchKey) {
    var search_input_value = $("#search_input").val();
    if (searchKey != null && searchKey != "")
        search_input_value = searchKey;
    $.ajax({
        url: "Home/InitializeRealInfoByWizardInfo",
        data: "searchKey=" + search_input_value,
        success: function (data) {
            mmg.load();
        }
    })
}
