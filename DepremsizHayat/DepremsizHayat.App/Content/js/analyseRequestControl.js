let requestButton = document.querySelector("#request")
requestButton.addEventListener("click", function (event) {
    let phone1 = document.querySelector("#phone1")
    let phone2 = document.querySelector("#phone2")
    let year = document.querySelector("#year")
    let floor = document.querySelector("#floor")
    let country = document.querySelector("#country")
    let district = document.querySelector("#district")
    let address = document.querySelector("#address")
    let note = document.querySelector("#note")

    let sendingFiles = new FormData()

    sendingFiles.append("phone1", phone1.value)
    sendingFiles.append("phone2", phone2.value)
    sendingFiles.append("year", year.value)
    sendingFiles.append("floor", floor.value)
    sendingFiles.append("country", country.value)
    sendingFiles.append("district", district.value)
    sendingFiles.append("address", address.value)
    sendingFiles.append("note", note.value)

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

//if (images.length < 0) { console.log("En az bir resim yüklemelisiniz") }
//else if (year.value == -1) { console.log("Yapım Yılı Seçiniz") }
//else if()


