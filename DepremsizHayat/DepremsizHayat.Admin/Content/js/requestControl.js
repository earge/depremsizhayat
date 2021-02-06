let floor = []
for (i = 1; i <= 20; i++) {
    floor.push(i)
}

let year = []
for (var i = 0; i <= 70; i++) {
    year.push((new Date).getFullYear() - i)
}

db = [{
    name: ["Rıdvan", "Önal"],
    surname: "Önal"
}]
$("#requestGrid").jsGrid({
    width: "100%",
    height: "400px",

    filtering: false,
    editing: true,
    sorting: false,
    paging: true,

    data: db,

    fields: [
        { title: "Ad", name: "FIRST_NAME", type: "text", width: 150, editing: false },
        { title: "Soyad", name: "LAST_NAME", type: "text", width: 50,editing:false },
        { title: "Ülke", name:"COUNTRY", type: "text", width: 200 },
        { title: "Şehir", name: "DISTRICT", type: "select"},
        { title: "Oluşturulma Tarihi", name: "CREATED_DATE", type:"text", editing: false },
        { title: "Adres", name: "ADDRESS", type:"textarea" },
        { title: "Kat Sayısı", name: "NUMBER_OF_FLOORS", type:"select",items:floor },
        { title: "Yapım Yılı", name: "YEAR_OF_CONSTRUCTION", type:"select",items:year },
        { title: "Telefon 1", name: "PHONE_NUMBER_1", type:"text" },
        { title: "Telefon 2", name: "PHONE_NUMBER_2", type:"text" },
        { title: "Açıklama", name: "USER_NOTE", type:"textarea" },
        { type: "control" }
    ]
});