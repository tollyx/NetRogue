using System;

namespace NetRogue.CMD {
    public struct Glyph : IEquatable<Glyph> {
        public char character;
        public ConsoleColor fore;
        public ConsoleColor back;

        public Glyph(char character) : this(character, ConsoleColor.White, ConsoleColor.Black) {}

        public Glyph(char character, ConsoleColor fore, ConsoleColor back) {
            this.character = character;
            this.fore = fore;
            this.back = back;
        }

        public override bool Equals(object obj) {
            if (!(obj is Glyph)) {
                return false;
            }

            var glyph = (Glyph)obj;
            return character == glyph.character &&
                   fore == glyph.fore &&
                   back == glyph.back;
        }

        public override int GetHashCode() {
            var hashCode = -2243911;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + character.GetHashCode();
            hashCode = hashCode * -1521134295 + fore.GetHashCode();
            hashCode = hashCode * -1521134295 + back.GetHashCode();
            return hashCode;
        }

        public bool Equals(Glyph other) {
            return character == other.character &&
                   fore == other.fore &&
                   back == other.back;
        }

        public static bool operator ==(Glyph a, Glyph b) {
            return a.Equals(b);
        }

        public static bool operator !=(Glyph a, Glyph b) {
            return !a.Equals(b);
        }
    }
}