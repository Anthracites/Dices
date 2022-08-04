// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("b8XCRCTXfeXyFy0wia7kH1TjijdNxdvhcA5kT66pkoBps0ZbiGMDXuBjbWJS4GNoYOBjY2LFLYoV5PcfCwtESKfQAgwD/n2TnyudTs6EpXNTGbDeCHFUPT3ckvYq33/X48zMvX9amh4NFcNxlIu6fSprPQ93icfEIP5soLUTpA+pLkFgTsquI8noQVhqhM74UfMgG6nTtrfHkKvSePYotVLgY0BSb2RrSOQq5JVvY2NjZ2JhEyHsNRyxiDPLssSu8GqxGKcup/fiagBiD4UOMno225wUs5mwrzcwbW7E2FiOVIAPZamkWDW5RyJay7/V1vZe4/6fP/2fzVjP8P/atSzgvf/6wzXHygRc951J6Iz2JGMbiEFiSOfPWQN2aO4SjWBhY2Jj");
        private static int[] order = new int[] { 6,5,5,3,4,10,13,8,13,12,10,11,12,13,14 };
        private static int key = 98;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
