let newName = document.querySelector("[name='newName']")
let newSurname = document.querySelector("[name='newSurname']")
let oldPassword = document.querySelector("[name='oldPassword']")
let newPassword = document.querySelector("[name='newPassword']")
let newMail = document.querySelector("[name='newMail']")

newName.onfocus = function () { newName.removeAttribute("readonly") }
newSurname.onfocus = function () { newSurname.removeAttribute("readonly") }
newPassword.onfocus = function () { newPassword.removeAttribute("readonly") }
oldPassword.onfocus = function () { oldPassword.removeAttribute("readonly") }
newMail.onfocus = function () { newMail.removeAttribute("readonly") }


document.querySelector("#edit").addEventListener("click", function (event) {
    var data =
    {
        Name: newName.value,
        Surname: newSurname.value,
        Mail: newMail.value
    }
    event.target.classList.add("loading")
    event.target.disabled = true
    $.ajax({
        data: data,
        url: "EditProfile",
        type: "POST",
        success: function (data) {
            event.target.classList.remove("loading")
            if (data.Status) {
                event.target.disabled = true
                setTimeout(function () { location.reload() }, 1000)
            }
            else { event.target.disabled = false }
            ResponseMessage("editJsonInfo",data)
        }
    })
})

