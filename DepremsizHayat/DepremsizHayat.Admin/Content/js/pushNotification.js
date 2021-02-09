const notificationTemplate = document.createElement("template")
notificationTemplate.innerHTML =
    `
<style>
:host{
  position:absolute;
  z-index:1600;
  left:50%;
  margin-top:-100px;
  opacity:0;
  animation: slideDown .4s alternate forwards linear,
             opacityUp .4s alternate forwards linear;
  max-width:300px;
  transform:translateX(-50%);
  font-family: Arial, Helvetica, sans-serif;
  font-size:14px;
}
:host([closing]){
  animation: slideUp .4s alternate forwards linear,
             opacityDown .4s alternate forwards linear;
}
:host > div:nth-of-type(1){
  padding:8px;
  border-radius:10px;
  background-color:rgba(255,255,255,1);
  box-shadow:inset 0px 0px 0px 1px rgba(28,28,28,.1),0px 0px 15px -5px rgba(28,28,28,0.2);
  display:flex;
}
:host > div:nth-of-type(1)::before{
  content:"";
  min-width:4px;
  border-radius:4px;
  margin-right:6px;
}
:host([type="error"]) > div:nth-of-type(1)::before{
  background-color:rgba(255,59,48,1);
}
:host([type="success"]) > div:nth-of-type(1)::before{
  background-color:rgba(59,199,89,1);
}
:host([type="warning"]) > div:nth-of-type(1)::before{
  background-color:rgba(255,204,0,1);
}
:host([type="info"]) > div:nth-of-type(1)::before{
  background-color:rgba(0,122,255,1);
}
@keyframes slideDown {
  from {margin-top:-100px}
  to {margin-top:10px}
}
@keyframes opacityUp {
  from {opacity:0}
  to {opacity:1}
}
@keyframes slideUp {
  from {margin-top:10px}
  to {margin-top:-100px}
}
@keyframes opacityDown {
  from {opacity:1}
  to {opacity:0}
}
</style>
<div><div></div></div>
`
class pushNotificaiton extends HTMLElement {
    constructor() {
        super()
        this.shadow = this.attachShadow({ mode: "closed" })
        this.shadow.appendChild(notificationTemplate.content.cloneNode(true))
    }
    get text() { return this._text }
    set text(string) { this._text = string }

    get type() { return this.getAttribute("type") }
    set type(type) { this.setAttribute("type", type) }

    get closeing() {
        return this.hasAttribute("closing")
    }

    set closing(bool) {
        if (bool) this.setAttribute("closing", "")
    }

    hideNotification = () => {
        this.closing = true
        setTimeout(() => { this.remove() }, 400)
    }

    connectedCallback() {
        this.shadow.querySelector(":host>div:nth-of-type(1)>div").innerHTML = this.text
        setTimeout(() => { this.hideNotification() }, 3000)
    }
}
customElements.define("ttx-notification", pushNotificaiton)

class Notification {
    static clearNotification() {
        Array.from(document.querySelectorAll("ttx-notification")).forEach(i => i.remove())
    }

    static push(type, text) {
        this.clearNotification()
        let pushElement = document.createElement("ttx-notification")
        pushElement.type = type
        pushElement.text = text
        document.body.prepend(pushElement)
    }
} 