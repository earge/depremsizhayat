let newName = document.querySelector("[name='newName']")
let newSurname = document.querySelector("[name='newSurname']")
let oldPassword = document.querySelector("[name='oldPassword']")
let newPassword = document.querySelector("[name='newPassword']")

newName.onfocus = function () { newName.removeAttribute("readonly") }
newSurname.onfocus = function () { newSurname.removeAttribute("readonly") }
newPassword.onfocus = function () { newPassword.removeAttribute("readonly") }
oldPassword.onfocus = function () { oldPassword.removeAttribute("readonly") }

document.querySelector("#edit").addEventListener("click", function (event) {
    var data =
    {
        Name: newName.value,
        Surname: newSurname.value
    }
    event.target.classList.add("loading")
    event.target.disabled = true
    $.ajax({
        data: data,
        url: "EditProfile",
        type: "POST",
        success: function (data) {
            if (data.Status) {
                event.target.disabled = true
                setTimeout(function () { location.reload() }, 1000)

                event.target.classList.remove("loading")
              

                document.querySelector("#editJsonInfo").classList.remove("none", "error", "success")
                document.querySelector("#editJsonInfo").classList.add("success")
                document.querySelector("#editJsonInfo").innerHTML = data.Message
            }
            else {
                event.target.classList.remove("loading")
                event.target.disabled = false
                document.querySelector("#editJsonInfo").classList.remove("none", "error", "success")
                document.querySelector("#editJsonInfo").classList.add("error")
                document.querySelector("#editJsonInfo").innerHTML = data.Message
            }
        }
    })
})