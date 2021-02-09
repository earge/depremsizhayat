

let newName = document.querySelector("input[name='newName']")
let newSurname = document.querySelector("input[name='newSurname']")
let newPassword = document.querySelector("input[name='newPassword']")
let newPasswordAgain = document.querySelector("input[name='newPasswordAgain']")
let newFIPassword = document.querySelector("#newPassword")
let newFIName = document.querySelector("#newName")
let newFISurname = document.querySelector("#newSurname")
newFIName.valid = true
newFISurname.valid = true

newPassword.onfocus = function () { newPassword.removeAttribute("readonly") }
newPasswordAgain.onfocus = function () { newPasswordAgain.removeAttribute("readonly") }

function checkAgain() {
    if (newPasswordAgain.value.trim().length != 0) {
        if (newPassword.value == newPasswordAgain.value) {
            document.querySelector(".extraSpan").classList.add("hidden")
            newPasswordAgain.classList.remove("border-red")
            return true

        } else {
            document.querySelector(".extraSpan").classList.remove("hidden")
            newPasswordAgain.classList.add("border-red")
            return false
        }
    }
}

newPasswordAgain.addEventListener("keyup", checkAgain)
newPassword.addEventListener("keyup", checkAgain)


document.querySelector("#edit").addEventListener("click", function (event) {
    if (newPassword.value.trim().length != 0) {
        if (checkAgain()) {
            if (newFIName.valid) {
                if (newFISurname.valid) {
                    ajaxForEditProfile(event) //Burası Şifreli
                } else {
                    newFISurname.Check()
                }
            } else {
                newFIName.Check()
            }
        }
    } else {
        if (newFIName.valid) {
            if (newFISurname.valid) {
                ajaxForEditProfile(event) //Burası Şifresiz
            } else {
                newFISurname.Check()
            }
        } else {
            newFIName.Check()
        }
    }    
})

function ajaxForEditProfile(event) {
    var data =
    {
        Name: newName.value,
        Surname: newSurname.value,
        Password: newPassword.value,
    }
    event.target.classList.add("loading")
    event.target.disabled = true
    $.ajax({
        data: data,
        url: "EditProfile",
        type: "POST",
        success: function (data) {
            event.target.classList.remove("loading")
            event.target.disabled = false
            //Array.from(document.querySelectorAll(".nameSurnameJson")).forEach(item=>item.innerHTML = )
            ResponseMessage("editJsonInfo", data)
        }
    })
}