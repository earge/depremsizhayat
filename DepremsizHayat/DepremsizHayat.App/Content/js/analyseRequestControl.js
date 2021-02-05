let requestButton = document.querySelector("#request")
requestButton.addEventListener("click", function (event) {
    let phone1 = document.querySelector("#phone1").value
    let phone2 = document.querySelector("#phone2").value
    let year = document.querySelector("#year").value
    let floor = document.querySelector("#floor").value
    let country = document.querySelector("#year").value
    let district = document.querySelector("#district").value
    let address = document.querySelector("#address").value

    let sendingFiles = new FormData()

    sendingFiles.append("phone1", phone1)
    sendingFiles.append("phone2", phone2)
    sendingFiles.append("year", year)
    sendingFiles.append("floor", floor)
    sendingFiles.append("country", country)
    sendingFiles.append("district", district)
    sendingFiles.append("address", address)

    for (i = 0; i < images.length; i++) {
        sendingFiles.append(images[i].name, images[i])
    }

    $.ajax({
        url: "/Home/SendAnalyseRequest",
        type: "POST",
        processData: false,
        contentType: false,
        data: sendingFiles,
        success: function () {
            debugger;
        },
        error: function () {
            debugger;
        }
    })
})




