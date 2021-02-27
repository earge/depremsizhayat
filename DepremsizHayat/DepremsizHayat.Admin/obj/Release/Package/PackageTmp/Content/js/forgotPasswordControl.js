document.querySelector("#forgot").addEventListener("click", function (event) {
    let formInput = document.querySelector("#mailForgot")
    if (formInput.valid == false) {
        formInput.Check()
    }
    else {
        var model = { Mail: document.querySelector("input[name='mailForgot']").value }
        Array.from(document.querySelectorAll(".info-boxes:not(.json)")).forEach(item => { item.remove() })
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

                    document.querySelector("#forgotJsonInfo").classList.remove("none","error","success")
                    document.querySelector("#forgotJsonInfo").classList.add("success")
                    document.querySelector("#forgotJsonInfo").innerHTML = data.Message      
                }
                else {
                    event.target.classList.remove("loading")
                    event.target.disabled = false

                    document.querySelector("#forgotJsonInfo").classList.remove("none", "error", "success")
                    document.querySelector("#forgotJsonInfo").classList.add("error")
                    document.querySelector("#forgotJsonInfo").innerHTML = data.Message 
                }
            },
            error: function (error) {

            }
        })
    }
})