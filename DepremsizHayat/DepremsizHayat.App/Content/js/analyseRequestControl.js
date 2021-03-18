
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

let loadProgress = document.querySelector("#loadProgress")
let uploadInfo = document.querySelector("#uploadInfo")
let progressLine = document.querySelector("#progressLine")

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

    if (images.length == 0) { requestErrorImage.classList.remove("hidden") }
    else if (year.value == "" || parseInt(year.value) < 1500 || parseInt(year.value) > (new Date).getFullYear()) { requestErrorYear.classList.remove("hidden") }
    else if (floor.value == "" || parseInt(floor.value) < 0) { requestErrorFloor.classList.remove("hidden") }
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
                progressLine.classList.remove("hidden")
            },
            processData: false,
            contentType: false,
            data: sendingFiles,
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                xhr.upload.onprogress = function (event) {
                    if (event.lengthComputable) {
                        var percentComplete = event.loaded / event.total;
                        loadProgress.value = Math.round(percentComplete * 100)
                        uploadInfo.innerHTML = Math.round(percentComplete * 100) + "%"
                    }
                }
                return xhr
            },
            success: function (data) {
                event.target.classList.remove("loading")
                event.target.disabled = false
                ResponseMessage("analyseJsonInfo", data)
                resetForm()
                window.location = "/Home/RequestSent"
            },
            error: function (err) {
                event.target.classList.remove("loading")
                event.target.disabled = false
                var singleData = {
                    Message: "Bir hata oluştu. Lütfen tekrar deneyin.",
                    Status: false
                }
                ResponseMessage("analyseJsonInfo", null, singleData)
            }
        })
    }
})

function resetForm() {
    phone1.value = ""
    phone2.value = ""
    year.value = ""
    floor.value = ""
    country.value = -1
    district.value = -1
    address.value = ""
    note.value = ""
    images = []
    document.querySelectorAll(".imagesBox>div>div").forEach(item => item.remove())
    document.querySelector(".imagesBox").classList.add("hidden")
    imgButton.disabled = false
    document.querySelector("#remaining").classList.add("hidden")
    progressLine.classList.add("hidden")
    setTimeout(function () { document.querySelector("#analyseJsonInfo").classList.add("hidden") }, 6000)
}

year.addEventListener("keypress", function () { requestErrorYear.classList.add("hidden") })
floor.addEventListener("change", function () { requestErrorFloor.classList.add("hidden") })
country.addEventListener("change", function () { requestErrorCountry.classList.add("hidden") })
district.addEventListener("change", function () { requestErrorDistrict.classList.add("hidden") })
address.addEventListener("keyup", function () { requestErrorAddress.classList.add("hidden") })
note.addEventListener("keyup", function () { requestErrorNote.classList.add("hidden") })
imageInput.addEventListener("change", function () { requestErrorImage.classList.add("hidden") })
