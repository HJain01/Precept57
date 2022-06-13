using Modding;
namespace Precept57
{
    internal abstract class Precept : Loggable
    {
        public abstract string SpriteFileName { get; }
        public EmbeddedSprite Sprite => new() {key = SpriteFileName};
        public abstract string Name { get; }
        public abstract string Id { get; }
        public abstract string ShopDescription { get; }
        public abstract string Take { get; }
        public abstract string Press { get; }
        public abstract string DescOne { get; }
        public abstract string DescTwo { get; }
        public abstract string Scene { get; }
        public abstract float X { get; }
        public abstract float Y { get; }
        public abstract PreceptSettings Settings(SaveSettings s);

        public virtual void Hook() {}
    }
}