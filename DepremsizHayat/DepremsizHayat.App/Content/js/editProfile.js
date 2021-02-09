let newName = document.querySelector("input[name='newName']")
let newSurname = document.querySelector("input[name='newSurname']")
let newPassword = document.querySelector("input[name='newPassword']")
let newPasswordAgain = document.querySelector("input[name='newPasswordAgain']")

newName.onfocus = function () { newName.removeAttribute("readonly") }
newSurname.onfocus = function () { newSurname.removeAttribute("readonly") }
newPassword.onfocus = function () { newPassword.removeAttribute("readonly") }
newPasswordAgain.onfocus = function () { newPasswordAgain.removeAttribute("readonly") }


document.querySelector("#edit").addEventListener("click", function (event) {
    let ajaxValid = false
    let passwordBox
    if (newPassword.value != "") {
        if (newPasswordAgain.value == newPassword.value) {
            document.querySelector(".extraSpan").classList.add("hidden")
            passwordBox = newPassword.value
            ajaxValid = true
        } else {
            document.querySelector(".extraSpan").classList.remove("hidden")
            ajaxValid = false
        }
    } else {
        passwordBox = null
        ajaxValid = true
    }

    var data =
    {
        Name: newName.value,
        Surname: newSurname.value,
        Password: passwordBox,
    }

    if (ajaxValid) {
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
                ResponseMessage("editJsonInfo", data)
            }
        })
    }
})

