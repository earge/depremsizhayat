let newName = document.querySelector("[name='newName']")
let newSurname = document.querySelector("[name='newSurname']")
let oldPassword = document.querySelector("[name='oldPassword']")
let newPassword = document.querySelector("[name='newPassword']")
newName.value = ""
newSurname.value = ""
newPassword.value = ""
oldPassword.value = ""

function getNameSurname() {
    $.ajax({
        url: "NameSurnameJson",
        type: "post",
        success: function (data) {
            newName.value = data.Name
            newSurname.value = data.Surname
        }
    })
}

getNameSurname()



function ajax() {
    var name = $("#name").val();
    var surname = $("#surname").val();
    var data = {
        Name: name,
        Surname: surname
    };
    $.ajax({
        data: data,
        url: "EditProfile",
        type: "POST",
        success: function (data) {
            debugger;
            alert(data.Message);
        }
    })
}