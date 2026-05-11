function fixFarsiStr(s1) {
  if (!s1) return '';

  let s2 = '';

  for (let i = 0; i < s1.length; i++) {
    const ch = s1[i];

    switch (ch) {

      case 'أ':
      case 'إ':
        s2 += 'ا';
        break;

      case 'ة':
        s2 += 'ه';
        break;

      // انواع ی
      case 'ؠ':
      case 'ؽ':
      case 'ؾ':
      case 'ؿ':
      case 'ٸ':
      case 'ی':
      case 'ۍ':
      case 'ێ':
      case 'ﯼ':
      case 'ﯽ':
      case 'ﻯ':
      case 'ﻰ':
      case 'ﻱ':
      case 'ﻲ':
      case 'ﻳ':
      case 'ﻴ':
      case 'ي':
        s2 += 'ی';
        break;

      // انواع ک
      case 'ک':
      case 'ك':
      case 'ؼ':
      case 'ڬ':
      case 'ڭ':
      case 'ڮ':
      case 'ݢ':
      case 'ݣ':
      case 'ݤ':
      case 'ﮎ':
      case 'ﮏ':
      case 'ﮑ':
        s2 += 'ک';
        break;

      // انواع و
      case 'و':
      case 'ؤ':
      case 'ٶ':
      case 'ٷ':
      case 'ۄ':
      case 'ۅ':
      case 'ۆ':
      case 'ۇ':
      case 'ۈ':
      case 'ۉ':
      case 'ۊ':
      case 'ۋ':
      case 'ݸ':
      case 'ݹ':
        s2 += 'و';
        break;

      default:
        const code = ch.charCodeAt(0);

        // unicode checks مثل C#
        if (code === 1740 || code === 1609) {
          s2 += 'ی';
        } else if (code === 1705) {
          s2 += 'ک';
        } else {
          s2 += ch;
        }
        break;
    }
  }

  return s2;
}
