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

    if (images.length == 0) { console.log("En az bir resim yüklemelisiniz") }
    else if (year.value == -1) { console.log("Yapım Yılı Seçiniz") }
    else if (floor.value == -1) { console.log("Kat Sayısını Seçiniz") }
    else if (country.value == -1) { console.log("Ülke Seçiniz") }
    else if (district.value == -1) { console.log("Şehir Seçiniz") }
    else if (address.value.trim().length == 0) { console.log("Adres Giriniz") }
    else if (note.value.trim().length == 0) { console.log("Açıklama Giriniz") }
    else {
        $.ajax({
            url: "/Home/SendAnalyseRequest",
            type: "POST",
            beforeSend: function () {
                event.target.classList.add("loading")
                event.target.disabled = true
            },
            processData: false,
            contentType: false,
            data: sendingFiles,
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                //Download progress
                xhr.addEventListener("progress", function (evt) {
                    console.log(evt.lengthComputable);
                    if (evt.lengthComputable) {
                        var percentComplete = evt.loaded / evt.total;
                        console.log(Math.round(percentComplete * 100) + "%")
                    }
                }, false);
                return xhr;
            },
            success: function (data) {
                event.target.classList.remove("loading")
                event.target.disabled = false
                ResponseMessage("analyseJsonInfo", data)
                resetForm()
            },
            
        })
    }
})

function resetForm() {
    document.querySelector("#phone1").value = ""
    document.querySelector("#phone2").value = ""
    document.querySelector("#year").value = -1
    document.querySelector("#floor").value = -1
    document.querySelector("#country").value = -1
    document.querySelector("#district").value=-1
    document.querySelector("#address").value=""
    document.querySelector("#note").value = ""
    images = []
    document.querySelectorAll(".imagesBox>div>div").forEach(item=>item.remove())
    document.querySelector(".imagesBox").classList.add("hidden")
}


