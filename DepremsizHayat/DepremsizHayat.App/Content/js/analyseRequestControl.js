let requestErrorImage = document.querySelector("#requestErrorImage")
let requestErrorYear = document.querySelector("#requestErrorYear")
let requestErrorFloor = document.querySelector("#requestErrorFloor")
let requestErrorCountry = document.querySelector("#requestErrorCountry")
let requestErrorDistrict = document.querySelector("#requestErrorDistrict")
let requestErrorAddress = document.querySelector("#requestErrorAddress")
let requestErrorNote = document.querySelector("#requestErrorNote")

let phone1 = document.querySelector("#phone1")
let phone2 = document.querySelector("#phone2")

let address = document.querySelector("#address")
let note = document.querySelector("#note")


let requestButton = document.querySelector("#request")
requestButton.addEventListener("click", function (event) {

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

    console.log(sendingFiles)

    if (images.length == 0) { requestErrorImage.classList.remove("hidden")}
    else if (year.value == -1) { requestErrorYear.classList.remove("hidden")  }
    else if (floor.value == -1) { requestErrorFloor.classList.remove("hidden") }
    else if (country.value == -1) { requestErrorCountry.classList.remove("hidden") }
    else if (district.value == -1) { requestErrorDistrict.classList.remove("hidden") }
    else if (address.value.trim().length == 0) { requestErrorAddress.classList.remove("hidden") }
    else if (note.value.trim().length == 0) { requestErrorNote.classList.remove("hidden") }
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
            //xhr: function () {
            //    var xhr = new window.XMLHttpRequest();
            //    //Download progress
            //    xhr.addEventListener("progress", function (evt) {
            //        console.log(evt.lengthComputable);
            //        if (evt.lengthComputable) {
            //            var percentComplete = evt.loaded / evt.total;
            //            console.log(Math.round(percentComplete * 100) + "%")
            //        }
            //    }, false);
            //    return xhr;
            //},
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
    phone1.value = ""
    phone2.value = ""
    year.value = -1
    floor.value = -1
    country.value = -1
    district.value=-1
    address.value=""
    note.value = ""
    images = []
    document.querySelectorAll(".imagesBox>div>div").forEach(item=>item.remove())
    document.querySelector(".imagesBox").classList.add("hidden")
    imgButton.disabled = false
    document.querySelector("#remaining").classList.add("hidden")
}

year.addEventListener("change", function () { requestErrorYear.classList.add("hidden") })
floor.addEventListener("change", function () { requestErrorFloor.classList.add("hidden") })
country.addEventListener("change", function () { requestErrorCountry.classList.add("hidden") })
district.addEventListener("change", function () { requestErrorDistrict.classList.add("hidden") })
address.addEventListener("keyup", function () { requestErrorAddress.classList.add("hidden") })
note.addEventListener("keyup", function () { requestErrorNote.classList.add("hidden") })
imageInput.addEventListener("change", function () { requestErrorImage.classList.add("hidden") })
