function ShenqingPeiban() {
    bootbox.confirm({
        buttons: {
            confirm: {
                label: '立刻申请成为陪伴师',
                className: 'btn-danger'
            },
            cancel: {
                label: '取消',
                className: 'btn-default'
            }
        },
        message: '你确定要申请成为陪伴师吗？',
        callback: function (result) {
            if (result) {
                var headers = {};
                //防伪标记放入headers
                //也可以将防伪标记放入data
                $.ajax({
                    url: url,
                    headers: headers,
                    type: 'post',
                    timeout: 3000,
                    data: {
                        rid: rid,
                        kid: kid,
                        status: 'true',
                    },
                    success: function (msg) {
                        if (msg.MessageStatus == 'true') {
                            window.location.href = tourl;
                        }
                        else {
                            alertconfirm('抱歉申请陪伴师失败！');
                        }
                    },
                    error: function (e) {
                        alertconfirm('出现意外错误，进入空间失败！');
                    }
                });
            }
            else {
                // alertconfirm('您已经取消删除了');
            }
        },
        title: "进入空间通行证",
    });
}
