using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfCommonds
{
    /// <summary>
    /// 拡大縮小時に綺麗に表示される画像クラス
    /// </summary>
    public class HighQualityImage : System.Windows.Controls.Image
    {
        protected override void OnRender(DrawingContext dc)
        {
            // VisualBitmapScalingMode は internal なので、継承する必要がある
            // http://msdn.microsoft.com/ja-jp/library/system.windows.media.bitmapscalingmode(v=vs.110).aspx
            this.VisualBitmapScalingMode = BitmapScalingMode.HighQuality;
            base.OnRender(dc);
        }
    }
}
