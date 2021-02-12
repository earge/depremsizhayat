let errorMessages = {
  requireName : "Adınız boş bırakılamaz.",
  shortName : "Adınız 2 karakterden uzun olmalıdır.",
  longName : "Adınız 40 karakterden kısa olmalıdır.",
  requireSurname : "Soyadınız boş bırakılamaz.",
  shortSurname : "Soyadınız 2 karakterden uzun olmalıdır.",
  longSurname : "Soyadınız 40 karakterden kısa olmalıdır.",
  requireMail : "E-Mailiniz boş bırakılamaz.",
  invalidMail : "Geçerli bir E-Mail giriniz.",
  requirePassword : "Şifreniz boş bırakılamaz",
  shortPassword : "Şifreniz 6 karakterden uzun olmalıdır.",
  longPassword : "Şifreniz 12 karaterden kısa olmalıdır.",
  invalidPassword : "Yeni şifreniz; en az bir küçük harf, en az bir büyük harf ve rakam içermelidir.",
  legalDate : "Yaşınız 18'den büyük olmalıdır.",
  invalidDate : "Geçerli bir tarih giriniz."
}

let months = ["Ocak","Şubat","Mart","Nisan","Mayıs","Temmuz","Haziran","Ağustos","Eylül","Ekim","Kasım","Aralık"]

class formInput extends HTMLElement{
  constructor(){
    super()
    this.errorBox
    this.inputBox
    this.selectBox = []
  }
  get valid(){return this._valid}
  set valid(bool){this._valid = bool;this.validChange()}

  get validType(){return this.getAttribute("valid-type")}
  set validType(type){this.setAttribute("valid-type",type)}

  get name(){return this.getAttribute("name")}
  set name(string){this.setAttribute("name",string)}

  get placeholder(){return this.getAttribute("placeholder")}
  set placeholder(string){this.setAttribute("placeholder",string)}

    get value() { return this.getAttribute("value") }
    set value(text) { this.setAttribute("value",text) }

  get invalid(){return this.hasAttribute("invalid")}

  set invalid(bool){
    if(bool) this.setAttribute("invalid","")
    else this.removeAttribute("invalid")
  }

  validChange(){
    if(!this.valid){
      this.appendChild(this.errorBox)
      this.invalid = true
    }
    else{
      this.errorBox.remove()
      this.invalid = false
    }
  }

  error(string) {
    this.valid = false 
    this.errorBox.innerHTML = string 
  }

  checkName(){
    if(this.inputBox.value.length == 0) { this.error(errorMessages.requireName) }
    else if(this.inputBox.value.length < 2){ this.error(errorMessages.shortName) }
    else if(this.inputBox.value.length > 40){ this.error(errorMessages.longName) }
    else{ this.valid=true }
  }

  checkSurname(){
    if(this.inputBox.value.length == 0) { this.error(errorMessages.requireSurname) }
    else if(this.inputBox.value.length < 2){ this.error(errorMessages.shortSurname) }
    else if(this.inputBox.value.length > 40){ this.error(errorMessages.longSurname) }
    else{ this.valid=true }
  }

  checkMail(){
    let regExp = new RegExp("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")
    if(this.inputBox.value.length == 0) { this.error(errorMessages.requireMail) }
    else if(!regExp.test(this.inputBox.value)){ this.error(errorMessages.invalidMail) }
    else{ this.valid=true }
  }

  checkPassword(){
    let regExp = new RegExp("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s).{6,12}$")
    if(this.inputBox.value.length == 0) { this.error(errorMessages.requirePassword) }
    else if(this.inputBox.value.length < 6){ this.error(errorMessages.shortPassword) }
    else if(this.inputBox.value.length > 12){ this.error(errorMessages.longPassword) }
    else if(!regExp.test(this.inputBox.value)) {this.error(errorMessages.invalidPassword)}
    else{ this.valid=true }
  }

  checkDate(){
    let day = this.selectBox[0].value
    let month = this.selectBox[1].value-1
    let year = this.selectBox[2].value

    let today = new Date()
    let todayDay = today.getDate()
    let todayMonth = today.getMonth()
    let todayYear = today.getFullYear()

    let date = new Date(year,month,day)
    if (date.getFullYear() == year && date.getMonth() == month && date.getDate() == day){
      let underAge = new Date(todayYear- 18, todayMonth, todayDay)
      if(date>underAge){ 
        this.error(errorMessages.legalDate);
        this.inputBox.value = ""
      }
      else{
        this.valid = true
        this.converToDate()
      }
    }else{
      this.error(errorMessages.invalidDate);
      this.inputBox.value = ""
    }
  }

  Check(){
    switch (this.validType) {
      case "name":
        this.checkName()
        this.inputBox.focus()
        break;
      case "surname":
        this.checkSurname()
        this.inputBox.focus()
        break;
      case "mail":
        this.checkMail()
        this.inputBox.focus()
        break;
      case "password":
        this.checkPassword()
        this.inputBox.focus()
        break;
      case "date":
        this.checkDate()
        break;
    }
  }

  RenderInput(){
    this.errorBox = document.createElement("span")
    this.inputBox = document.createElement("input")
    this.inputBox.placeholder = this.placeholder
    this.inputBox.name = this.name
    this.inputBox.value = this.value
    this.appendChild(this.inputBox)
  }

  RanderDate(){
    let today = new Date()
    let todayDay = today.getDate()
    let todayMonth = today.getMonth()+1
    let todayYear = today.getFullYear()

    let dateContainer = document.createElement("div")
    dateContainer.classList.add("flex","h-stretch-100")
    this.inputBox = document.createElement("input")
    this.errorBox = document.createElement("span")
    this.selectBox[0] = document.createElement("select")
    this.selectBox[1] = document.createElement("select")
    this.selectBox[2] = document.createElement("select")
    this.selectBox[0].classList.add("select","one","mr7","pl10")
    this.selectBox[1].classList.add("select","one","mr7","ml7","pl10")
    this.selectBox[2].classList.add("select","one","ml7","pl10")
    for(var index=1;index<=31;index++){
      let option = document.createElement("option")
      option.value = index
      option.innerHTML = index
      this.selectBox[0].appendChild(option)
    }
    Array.from(months).forEach((month,index)=>{
      let option = document.createElement("option")
      option.value = index+1
      option.innerHTML = month
      this.selectBox[1].appendChild(option)
    })
    for(var index=0;index<=100;index++){
      let option = document.createElement("option")
      option.value = todayYear-index
      option.innerHTML = todayYear-index
      this.selectBox[2].appendChild(option)
    }
    this.selectBox[0].value = todayDay
    this.selectBox[1].value = todayMonth
    this.inputBox.name = this.name
    Array.from(this.selectBox).forEach(item=>dateContainer.appendChild(item))
    this.appendChild(dateContainer)
    this.appendChild(this.inputBox)
  }

  converToDate(){
    let yearResult = this.selectBox[2].value
    let monthResult = (this.selectBox[1].value<10?"0":"")+this.selectBox[1].value
    let dayResult = (this.selectBox[0].value<10?"0":"")+this.selectBox[0].value
    this.inputBox.value = `${yearResult}-${monthResult}-${dayResult}`
  }
  
  connectedCallback(){
    if(!this.validType) this.remove()
    switch (this.validType) {
      case "name":
        this.RenderInput()
        this.inputBox.type = "text"
        this.inputBox.addEventListener("blur",this.checkName.bind(this))
        this.inputBox.addEventListener("keyup",this.checkName.bind(this))
        this.inputBox.addEventListener("change",this.checkName.bind(this))
        break;
      case "surname":
        this.RenderInput()
        this.inputBox.type = "text"
        this.inputBox.addEventListener("blur",this.checkSurname.bind(this))
        this.inputBox.addEventListener("keyup",this.checkSurname.bind(this))
        this.inputBox.addEventListener("change",this.checkSurname.bind(this))
        break;
      case "mail":
        this.RenderInput()
        this.inputBox.type = "text"
        this.inputBox.addEventListener("blur",this.checkMail.bind(this))
        this.inputBox.addEventListener("keyup",this.checkMail.bind(this))
        this.inputBox.addEventListener("change",this.checkMail.bind(this))
        break;
      case "password":
        this.RenderInput()
        this.inputBox.type = "password"
        this.inputBox.addEventListener("blur",this.checkPassword.bind(this))
        this.inputBox.addEventListener("keyup",this.checkPassword.bind(this))
        this.inputBox.addEventListener("change",this.checkPassword.bind(this))
        break;
      case "date":
        this.RanderDate()
        this.inputBox.type = "hidden"
        this.selectBox[0].addEventListener("change",this.checkDate.bind(this))
        this.selectBox[1].addEventListener("change",this.checkDate.bind(this))
        this.selectBox[2].addEventListener("change",this.checkDate.bind(this))
        break;
    }
  }  
}
customElements.define("form-input",formInput)