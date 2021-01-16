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
            dataType: "application/JSON",
            type: "POST",
            success: function (data) {
                console.log("ok");
            },
            error: function (data) {
                console.log(data.statusText)
            }
        })
    }
})