namespace Precept57
{
    internal abstract class Precept
    {
        public abstract string Sprite { get; }
        public abstract string Name { get; }
        public abstract string Id { get; }
        public abstract string Description { get; }
        public abstract string Take { get; }
        public abstract string Press { get; }
        public abstract string DescOne { get; }
        public abstract string DescTwo { get; }
        public abstract string Scene { get; }
        public abstract float X { get; }
        public abstract float Y { get; }
        public bool Equipped() => PlayerData.instance.GetBool(Id);
        public abstract PreceptSettings Settings(SaveSettings s);

        public virtual void Hook() {}
    }
}