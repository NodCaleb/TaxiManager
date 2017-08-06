//Скрипты для выполнения AJAX-запросов (в основном)

//Загрузка теущих заявок
function loadRequests() {
    //Показываем крутилку
    $('#data-table').replaceWith('<div id="data-table"><div style="text-align:center; margin:80px;"><img src="/Images/loadingimg.gif" /><br />Загрузка...</div></div>')
    //Запрашиваем перечень заявок
    $.ajax({
        type: 'POST',
        url: "/Ajax/GetRideRequests"
    }).done(function (data) {
        //Если все в порядке - показываем списко заявок
        $('#data-table').replaceWith(data);
    }).fail(function (data) {
        //Если не в порядке - показываем сообщение об ошибке
        $('#data-table').replaceWith('<div id="data-table"><div style="text-align:center; margin:80px;"><img width="200" src="/Images/error.png" /><br />Ошибка сервера!</div></div>');
    });
};
function createRequset() {
    var valid = true;
    //Проверяем заполнение полей
    if ($("#create-start").val() == "") {
        $("#create-start").parent().addClass("has-error");
        valid = false;
    }
    else {
        $("#create-start").parent().removeClass("has-error");
    };

    if ($("#create-destination").val() == "") {
        $("#create-destination").parent().addClass("has-error");
        valid = false;
    }
    else {
        $("#create-destination").parent().removeClass("has-error");
    };
    //Если заполенно верно - создаем заявку
    if (valid) {
        //Прячем форму и кнопки, показываем крутилку
        $('#create-body').hide();
        $('#create-progress').show();
        $('#create-footer').hide();
        $.ajax({
            type: 'POST',
            url: "/Ajax/CreateRideRequest/",
            data: {
                Start: $('#create-start').val(),
                Destination: $('#create-destination').val()
            }
        }).done(function (data) {
            if (data.Id != undefined) {
                //Если все в порядке - прячем модал и обновляем список
                $('#createModal').modal('hide');
                $('form#create-form input').val('');
                loadRequests();
            }
            else {
                //Если не в порядке - выдаем сообщение об ошибке и снова показываем форму
                $('#create-body').show();
                $('#create-progress').hide();
                $('#create-footer').show();
                alert(data.ErrorMessage);
            };
        });
    };
};
//Устанавливаем статус заявки
function setRequestStatus(id, status) {
    $.ajax({
        type: 'POST',
        url: "/Ajax/EditRideRequest/",
        data: {
            Id: id,
            Status: status
        }
    }).done(function (data) {
        if (data.Id != undefined) {
            loadRequests();
        }
        else {
            alert(data.ErrorMessage);
        };
    });
};
//Назначаем водителя на заявку
function assignDriver(id, driverId) {
    $.ajax({
        type: 'POST',
        url: "/Ajax/EditRideRequest/",
        data: {
            Id: id,
            DriverId: driverId,
            Status: -1
        }
    }).done(function (data) {
        if (data.Id != undefined) {
            loadRequests();
        }
        else {
            alert(data.ErrorMessage);
        };
    });
};
//Отказ водителя от заявки
function rejectRequest(id) {
    $.ajax({
        type: 'POST',
        url: "/Ajax/EditRideRequest/",
        data: {
            Id: id,
            Status: -1,
            Reject: "True"
        }
    }).done(function (data) {
        if (data.Id != undefined) {
            loadRequests();
        }
        else {
            alert(data.ErrorMessage);
        };
    });
};
//Загружаем список пользователей
function loadUsers() {
    $('#data-table').replaceWith('<div id="data-table"><div style="text-align:center; margin:80px;"><img src="/Images/loadingimg.gif" /><br />Загрузка...</div></div>')

    $.ajax({
        type: 'POST',
        url: "Ajax/GetUsers/"
    }).done(function (data) {
        $('#data-table').replaceWith(data);
    }).fail(function (data) {
        $('#data-table').replaceWith('<div id="data-table"><div style="text-align:center; margin:80px;"><img width="200" src="/Images/error.png" /><br />Ошибка сервера!</div></div>');
    });
};
//Создаем пользователя
function createUser() {
    var valid = true;

    if ($("#create-username").val() == "") {
        $("#create-username").parent().addClass("has-error");
        valid = false;
    }
    else {
        $("#create-username").parent().removeClass("has-error");
    };

    if ($("#create-email").val() == "") {
        $("#create-email").parent().addClass("has-error");
        valid = false;
    }
    else {
        $("#create-email").parent().removeClass("has-error");
    };

    if ($("#create-password").val() == "") {
        $("#create-password").parent().addClass("has-error");
        valid = false;
    }
    else {
        $("#create-password").parent().removeClass("has-error");
    };

    if ($("#create-personalname").val() == "") {
        $("#create-personalname").parent().addClass("has-error");
        valid = false;
    }
    else {
        $("#create-personalname").parent().removeClass("has-error");
    };

    if ($("#create-role").val() == "") {
        $("#create-role").parent().addClass("has-error");
        valid = false;
    }
    else {
        $("#create-role").parent().removeClass("has-error");
    };

    if (valid) {
        $('#create-body').hide();
        $('#create-progress').show();
        $('#create-footer').hide();
        $.ajax({
            type: 'POST',
            url: "/Ajax/CreateUser/",
            data: {
                UserName: $('#create-username').val(),
                Email: $('#create-email').val(),
                PersonalName: $('#create-personalname').val(),
                Password: $('#create-password').val(),
                Role: $('#create-role').val()
            }
        }).done(function (data) {
            if (data.Id != undefined) {
                $('#createModal').modal('hide');
                $('form#create-form input').val('');
                loadUsers();
            }
            else {
                $('#create-body').show();
                $('#create-progress').hide();
                $('#create-footer').show();
                alert(data.ErrorMessage);
            };
        });
    };
};
//Назначаем роль пользователю
function assignRole(id, role) {
    $.ajax({
        type: 'POST',
        url: "/Ajax/AssignRole/",
        data: {
            Id: id,
            Role: role
        }
    }).done(function (data) {
        if (data.Id != undefined) {
            loadUsers();
        }
        else {
            alert(data.ErrorMessage);
        };
    });
};
$(function () {
    //При открытии модала на создание заявки или пользователя - отборажаем форму и кнопки, прячем крутилку
    $('#createModal').on('show.bs.modal', function (e) {
        $('#create-body').show();
        $('#create-progress').hide();
        $('#create-footer').show();
    });
});