using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Examples.Engine2D.Spaceship.Core;
using TGC.Core.Input;
using TGC.Core.Text;
using Microsoft.DirectX;
namespace TGC.Group.Model.Menu
{
    
    public abstract class MenuObject
    {
        public abstract void Update(float elapsedTime);
        public abstract void Render(float elapsedTime, Drawer2D drawer);

        protected Core.Input.TgcD3dInput Input;
        public MenuObject(TgcD3dInput Input) { this.Input = Input; }
    }


    class Menu : MenuObject
    {
        Drawer2D drawer;
        String Titulo;
        List<Button> mBotones;
        #region MenuObject methods
        public override void Update(float elapsedTime)
        {
            mBotones.ForEach((Button b) => { b.Update(elapsedTime); });
        }
        public override void Render(float elapsedTime, Drawer2D drawer)
        {
            mBotones.ForEach((Button b) => { b.Render(elapsedTime, drawer); });
        }
        #endregion

        public void pushButton(Button button) { mBotones.Add(button); }
        public void removeButton(Button button) { mBotones.Remove(button); }
        public Menu(TgcD3dInput Input,String Titulo) : base(Input) {
            this.mBotones = new List<Button>();
            this.Titulo = Titulo;
        }

    }

    class Button:MenuObject
    {

        CustomBitmap sprite_normal;
        CustomBitmap sprite_mouseover;
        CustomSprite current_sprite;
        Action Callback { get; set; }
        String nombre;
        Vector2 Size {
            get {
                var vec = new Vector2(this.current_sprite.SrcRect.Width, this.current_sprite.SrcRect.Height);
                vec.X *= this.current_sprite.Scaling.X;
                vec.Y *= this.current_sprite.Scaling.Y;
                return vec;
            }
        }   
        public Vector2 Position { get { return current_sprite.Position; } set { current_sprite.Position=value; } }

        #region GameObject Methods
        public override void Update(float elapsedTime)
        {
            var mousePos = new Vector2(Input.Xpos, Input.Ypos);
            var minVec = this.Position;
            var maxVec = this.Position + this.Size;
            //chequeamos si el mouse está sobre del botón
            if (minVec.X < mousePos.X && minVec.Y < mousePos.Y &&
               mousePos.Y < maxVec.Y && mousePos.Y < maxVec.Y)
            {
                //si el mouse está sobre el botón lo resaltamos
                current_sprite.Bitmap = sprite_mouseover;
                //si se presiona el botón callbackeamos el callback.
                if (Input.buttonPressed(TgcD3dInput.MouseButtons.BUTTON_LEFT)) {
                    Callback();
                }
            }//si el mouse no está sobre el botón lo dejamos normal
            else current_sprite.Bitmap = sprite_normal;
            
        }
        public override void Render(float elapsedTime, Drawer2D drawer)
        {//renderizamos el sprite del botón
            drawer.DrawSprite(current_sprite);
        }
        #endregion

        
        public Button(TgcD3dInput Input, CustomBitmap normal, CustomBitmap mouseover, Action Callback) : base(Input) {
            this.sprite_normal = normal;
            this.sprite_mouseover = mouseover;
            this.Callback = Callback;
        }
    }
}
