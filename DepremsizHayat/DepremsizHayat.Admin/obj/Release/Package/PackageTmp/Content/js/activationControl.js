document.querySelector("#activate").addEventListener("click", function () {
    let digits = Array.from(document.querySelector("#digits").children)
    let empty = digits.filter(item => item.value == "").length
    let queryString = window.location.search.split('&')[0]
    if (empty == 0) {
        let numbers=""
        digits.forEach(item => { numbers += item.value })
        queryString += numbers + "&" + window.location.search.split('&')[1]
        window.location=queryString
    } else {
        console.log("Empty")
    }

});