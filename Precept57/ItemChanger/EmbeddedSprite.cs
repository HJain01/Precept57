using ItemChanger;
using Precept57;
using UnityEngine;

namespace Precept57
{
    internal class EmbeddedSprite : ISprite
    {
        public string key;

        [Newtonsoft.Json.JsonIgnore]
        public Sprite Value => EmbeddedSprites.Get(key);
        public ISprite Clone() => (ISprite)MemberwiseClone();
    }
}