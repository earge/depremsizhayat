let newName = document.querySelector("[name='newName']")
let newSurname = document.querySelector("[name='newSurname']")
let oldPassword = document.querySelector("[name='oldPassword']")
let newPassword = document.querySelector("[name='newPassword']")

newName.onfocus = function() { newName.removeAttribute("readonly") }
newSurname.onfocus = function () { newSurname.removeAttribute("readonly") }
newPassword.onfocus = function () { newPassword.removeAttribute("readonly") }
oldPassword.onfocus = function () { oldPassword.removeAttribute("readonly") }





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