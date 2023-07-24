
using WebBanHangOnline.Data;

namespace WebBanHangOnline.Common
{
	public class SettingHelper
	{
		private readonly ApplicationDbContext _context;

		public SettingHelper(ApplicationDbContext context)
		{
			_context = context;
		}
		public string GetValue(string key)
		{
			var item =  _context.SystemSetting.SingleOrDefault(x => x.SettingKey == key);
			if (item != null)
			{
				return item.SettingValue;
			}
			return "";
		}
	}
}
