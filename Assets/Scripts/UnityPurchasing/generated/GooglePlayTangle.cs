// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("iKgAvaDBYaPBkwaRrqGE63K+46G+PTM8DL49Nj6+PT08m3PUS7qpQTTakKYPrX5F943o6ZnO9YwmqHbrDUfugFYvCmNjgsyodIEhib2SkuMTm4W/LlA6EfD3zN437RgF1j1dAKSda5mUWgKpwxe20qh6PUXWHzwWfqAy/utN+lH3cB8+EJTwfZe2HwYhBMRAU0udL8rV5CN0NWNRKdeZmk1/smtC79Ztleya8K4070b5cPmpMJqGBtAK3lE79/oGa+cZfASV4Yu8NF48UdtQbCRohcJK7cfu8WluMwy+PR4MMTo1Frp0ussxPT09OTw/VVUaFvmOXFJdoCPNwXXDEJDa+y0xm5waeokju6xJc27X8LpBCr3UabmRB10oNrBM0z4/PTw9");
        private static int[] order = new int[] { 9,1,8,4,5,7,13,10,12,11,12,11,12,13,14 };
        private static int key = 60;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
