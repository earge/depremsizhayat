const promptTemplate = document.createElement("template")
        promptTemplate.innerHTML = 
        `
        <style>
        :host{
            position:absolute;
            top:0px;
            left:0px;
            z-index:999;
            width:100%;
            height:100%;
            background-color:rgba(0,0,0,0.1);
            display:flex;
            justify-content:center;
            backdrop-filter:blur(5px);    
        }
        :host > div {
            margin:10px ;
            flex: 1 1 auto;
            max-width:600px;
            height:200px;
            width:100%;
            background-color:white;
            border-radius:10px;
            box-shadow:0px 0px 15px -5px rgba(0,0,0,0.1)
        }
        </style>
        <div>
            <div>Kapat</div>
            <div>
                <slot></slot>
            </div>
        </div>
        `
        class Prompt extends HTMLElement{
            constructor(){
                super()
                this.shadow = this.attachShadow({mode:"closed"})
                this.shadow.appendChild(promptTemplate.content.cloneNode(true))
            }
            get closing(){
                return this.hasAttribute("closing")
            }
            set closing(bool){
                if(bool) this.setAttribute("closing","")
                else this.removeAttribute("closing")
            }
            close = () => {
                this.closing = true
                setTimeout(()=>{this.remove()},310)
            }
            static show (template) {
                let prompt = new Prompt()
                prompt.appendChild(template.content.cloneNode(true))
                document.body.appendChild(prompt)
            }
            connectedCallback(){
                this.shadow.querySelector(":host>div>div:nth-of-type(1)").addEventListener("click",this.close.bind(this))
            }
        }
        customElements.define("ro-prompt",Prompt)