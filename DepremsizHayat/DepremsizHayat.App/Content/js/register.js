document.querySelector("#register").addEventListener("click", (event) => {
    let formInputs = Array.from(document.querySelectorAll("form-input")).filter(item => item.valid != true)
    if (formInputs.length != 0) { formInputs.shift().Check() }
    else {
        event.target.classList.add("loading")
        event.target.disabled = true
        let registerModel = {
            FIRST_NAME: document.querySelector("input[name='name']").value,
            LAST_NAME: document.querySelector("input[name='lastname']").value,
            PASSWORD: document.querySelector("input[name='password']").value,
            E_MAIL: document.querySelector("input[name='email']").value
        }
        $.ajax({
            url: "Register",
            data: registerModel,
            type: "POST",
            success: function (data) {
                if (data.Status) {
                    window.location = "/Account/Activate"+data.Url
                }
                else {
                    event.target.classList.remove("loading")
                    event.target.disabled = false
                    document.querySelector("#registerJsonError").classList.remove("none")
                    document.querySelector("#registerJsonError").innerHTML = data.Message
                }
            },
            error: function (data) {
                debugger;
                window.location = "/Account/Register"
            }
        })
    }
})