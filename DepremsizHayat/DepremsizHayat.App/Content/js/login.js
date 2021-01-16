document.querySelector("#loginButton").addEventListener("click", function (event) {
    let email = document.querySelector("#email").value
    let password = document.querySelector("#password").value
    let loginModel = {
        E_MAIL: email,
        PASSWORD: password
    }
    if (email && password) {
        event.target.classList.add("loading")
        event.target.disabled = true
        $.ajax({
            url: "Login",
            data: loginModel,
            type: "POST",
            dataType: "application/JSON",
            success: function (data) {
                console.log("başarılı")
            },
            error: function (data) {
                console.log(data.statusText)
            }
        })
    }
})