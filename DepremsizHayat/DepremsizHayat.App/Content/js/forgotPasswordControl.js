document.querySelector("#forgot").addEventListener("click", function (event) {
    let formInput = document.querySelector("#mailForgot")
    if (formInput.valid == false) {
        formInput.Check()
    }
    else {
        var model = { Mail: document.querySelector("input[name='mailForgot']").value }
        event.target.classList.add("loading")
        event.target.disabled = true
        $.ajax({
            url: "SendForgotLink",
            data: model,
            type: "POST",
            success: function (data) {
                if (data.Status) {
                    event.target.classList.remove("loading")
                    event.target.disabled = false
                    document.querySelector("#forgotJsonSuccess").classList.remove("none")
                    document.querySelector("#forgotJsonSuccess").innerHTML = data.Message

                    document.querySelector("#forgotJsonError").classList.add("none")
                    document.querySelector("#forgotJsonError").innerHTML = ""
                }
                else {
                    debugger;
                    event.target.classList.remove("loading")
                    event.target.disabled = false
                    document.querySelector("#forgotJsonError").classList.remove("none")
                    document.querySelector("#forgotJsonError").innerHTML = data.Message

                    document.querySelector("#forgotJsonSuccess").classList.add("none")
                    document.querySelector("#forgotJsonSuccess").innerHTML = ""
                }
            },
            error: function (error) {

            }
        })
    }
})