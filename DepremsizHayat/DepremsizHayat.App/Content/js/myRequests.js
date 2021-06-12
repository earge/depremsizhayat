getMyRequest(1)

function getMyRequest(page) {
    $.ajax({
        url: "/Home/MyRequests_",
        contentType: false,
        type: "POST",
        beforeSend: function () {
            document.querySelector("#my-request-loading").classList.remove("hidden")
        },
        success: function (data) {
            var length = data.length
            document.querySelector("#my-request-loading").classList.add("hidden")
            if (data.length == 0) {
                document.querySelector(".container").innerHTML =
                    `
                        <div class="align-center b fs-14 mb10">Henüz bir talepte bulunmamışsınız.</div>
                        <div class="align-center"><button id="go-to-request" class="btn btn-main btn-blue">Talep Oluştur</button></div>
                    `
                document.querySelector("#go-to-request").addEventListener("click", function () {
                    window.location = "/Home/Index"
                })
            }
            data.forEach((data, index) => {

                let color = ""
                if (data.STATUS_CODE == "waitingadmconf") {
                    color = "orange"
                }
                else if (data.STATUS_CODE == "denied") {
                    color = "red"
                }
                else if (data.STATUS_CODE == "sent") {
                    color = "blue"
                }
                else if (data.STATUS_CODE == "completed") {
                    color = "green"
                }
                else if (data.STATUS_CODE == "waitingexpconf") {
                    color = "yellow"
                }
                var requestBox =
                    `
                        <div class="request-box fs-14 white-box">
                            <div class="p5">
                                <div class="status ${color}">${data.STATUS_NAME}</div>

                                <div class="line mt10">
                                    <div class="flex h-stretch-100"><div class="p7"><b>Ülke: </b>${data.COUNTRY}</div> <div class="p7"><b> Şehir: </b>${data.DISTRICT}</div></div>
                                    <div class="p7"><b>Adres: </b>${data.ADDRESS}</div>
                                </div>

                                <div class="line mt10">
                                    <div class="flex h-stretch-100"><div class="p7"><b>Bina Kat Sayısı: </b>${data.NUMBER_OF_FLOORS}</div> <div class="p7"><b>Bina Yapım Yılı: </b>${data.YEAR_OF_CONSTRUCTION}</div></div>
                                </div>
                                <div class="request-button-container mt10"><button data-request-id="${data.ANALYSIS_REQUEST_ID}" class="request-btn">Daha Fazla Bilgi</button></div>
                            </div>
                        </div>
                    `
                if (length > 1 && index < length - 1) requestBox += `<div class="seperator"></div>`
                document.querySelector(".container").innerHTML += requestBox
            })
            Array.from(document.querySelectorAll(".request-btn")).forEach(item => {
                item.addEventListener("click", function () { getMyRequestDetail(item.dataset.requestId) })
            })

        },
        error: function () {
            document.querySelector(".container").innerHTML = `<div class="align-center tx-red">Talebleriniz gösterilirken bir hata oluştu.<div>`
        }
    })
}

function getMyRequestDetail(dataRequestId) {
    let templateString = ""
    let template = document.createElement("template")


    $.ajax({
        url: "/Home/RequestDetail?requestId=" + dataRequestId,
        type: "POST",
        contentType: false,
        success: function (data) {
            console.log(data)
            template.innerHTML = "<h3>Talep Detayları</h3>"
            templateString +=
                `
                    <table id="prompt-request-detail">
                        <tr>
                            <td>Oluşturulma Tarihi</td>
                            <td>${ConvertDate(data.CREATED_DATE)}</td>
                        </tr>
                        <tr>
                            <td>Adres</td>
                            <td>${data.ADDRESS}</td>
                        </tr>
                    `
            if (data.PHONE_NUMBER_1) {
                templateString +=
                    `
                        <tr>
                            <td>Telefon</td>
                            <td>${data.PHONE_NUMBER_1}</td>
                        </tr>
                        `
            }

            if (data.PHONE_NUMBER_2) {
                templateString +=
                    `
                        <tr>
                            <td>Diğer Telefon</td>
                            <td>${data.PHONE_NUMBER_2}</td>
                        </tr>
                        `
            }
            templateString +=
                `
                        <tr>
                            <td>Açıklama</td>
                            <td>${data.USER_NOTE}</td>
                        </tr>
                    `

            if (data.ANSWER) {
                templateString +=
                    `<tr>
                            <td>Uzman Yanıtı</td>
                            <td>${data.ANSWER}</td>
                        </tr>`
            }
            if (data.RISK_SCORE) {
                templateString +=
                    `<tr>
                            <td>Uzman Yanıtı</td>
                            <td>${data.RISK_SCORE}</td>
                        </tr>`
            }

            templateString += `
                        </table>
                        <div>
                        <h3 style="margin:0px" class="pl10 pb5 pt15">Binaya Ait Görseller</h3>

                        </div>

                        <div class="row">
                          ${imagesToHtml(data.IMAGES)}
                        </div>

                        <div class="image-container">
                          <span onclick="this.parentElement.style.display='none'" class="closebtn">&times;</span>
                          <img id="expandedImg" style="width:100%">
                          <div id="imgtext"></div>
                        </div>

                    `
            template.innerHTML += templateString
            Prompt.show(template)
        }
    })
}

function imagesToHtml(images) {
    let imageString = ""
    Array.from(images).forEach(item => {
        imageString += `
                    <div class="column">
                         <img src="${item}" onclick="myFunction(this);">
                    </div>
                    `
    })
    return imageString
}

function myFunction(imgs) {
    var expandImg = document.getElementById("expandedImg");
    var imgText = document.getElementById("imgtext");
    expandImg.src = imgs.src;
    imgText.innerHTML = imgs.alt;
    expandImg.parentElement.style.display = "block";
}