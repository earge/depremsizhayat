document.querySelector("#forgot").addEventListener("click", function (event) {
    let formInput = document.querySelector("#mailForgot")
    if (formInput.valid == false) {
        formInput.Check()
    }
    else {
        var model = { Mail: document.querySelector("input[name='mailForgot']").value }
        infoBoxesCleaner()
        event.target.classList.add("loading")
        event.target.disabled = true
        $.ajax({
            url: "SendForgotLink",
            data: model,
            type: "POST",
            success: function (data) {
                event.target.classList.remove("loading")
                event.target.disabled = false
                ResponseMessage("forgotJsonInfo",data)
            },
            error: function (error) {

            }
        })
    }
})