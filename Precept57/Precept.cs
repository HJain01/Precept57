namespace Precept57
{
    internal abstract class Precept
    {
        public abstract string Sprite { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Scene { get; }
        public abstract float X { get; }
        public abstract float Y { get; }

        // assigned at runtime by SFCore's CharmHelper
        public int Num { get; set; }


        public abstract PreceptSettings Settings(SaveSettings s);

        public virtual void Hook() {}
    }
}