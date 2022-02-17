using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartlink;

public class Translation {
    public Translation(string name) {
        States[0] = name;
    }
    public string Name { get {
            return States[0];
        }
    }
    public string[] States = new string[3];
}

public class TranslationGroup : Dictionary<int, Translation> {
    public string GetString(int key, int index) {
        // MPL_2310_Oткpыт_Зaкpыт$$Paзp. Пycк Ocн. Moтopa
        if (ContainsKey(key)) {
            var t = this[key];
            if (index < t.States.Length)
                return this[key].States[index];
        }
        return $"key:{key}, index:{index}";
    }
    public string GetString(int key) {
        // MPL_2310_Oткpыт_Зaкpыт$$Paзp. Пycк Ocн. Moтopa
        if (ContainsKey(key)) 
            return this[key].Name;        
        return $"key:{key}";
    }

    public void AddTranslation(int key, Translation t) {
        if (!ContainsKey(key)) {
            Add(key, t);
        }
        else {
            this[key] = t;
        }
    }
}

public class LanguageLoader {
    public void AddString(string line, Language language) {
        if (string.IsNullOrEmpty(line)) 
            return;
        // example: MPL_2310_Oткpыт_Зaкpыт$$Paзp. Пycк Ocн. Moтopa
        var parts = line.Split("$$");
        if (parts.Length > 1) {
            // MPL_2310_Oткpыт_Зaкpыт
            string[] info = parts[0].Split('_');
            // MPL
            string type = info[0];
            // 2310
            var idx = int.Parse(info[1]); // система счисления 10
            // 4
            var argc = info.Length;
            var strings = new string[argc - 1];
            // Paзp. Пycк Ocн. Moтopa
            strings[0] = parts[1];
            Translation t = new Translation(parts[1]);
            if (argc >= 3) {
                // Открыт
                strings[1] = info[2];
                // Закрыт
                strings[2] = info[3];
                t.States[1] = info[2];
                t.States[2] = info[3];
            }
            var key = idx;
            language.AddTranslation(type, key, t);
        }
    }

    public void LoadStrings(string[] lines, Language language) {
        foreach (var line in lines) 
            AddString(line, language);
    }
    public void LoadLanguage(string filename, Language language) {
        string[] lines = File.ReadAllLines(filename);
        LoadStrings(lines, language);
    }
}

public class Language : Dictionary<string, TranslationGroup> {
    public int StringCount {
        get {
            int count = 0;
            foreach (TranslationGroup g in Values) 
                count += g.Values.Count;
            return count;                
        }
    }
    public string GetString(string type, int key, int index) {
        // MPL_2310_Oткpыт_Зaкpыт$$Paзp. Пycк Ocн. Moтopa
        if (ContainsKey(type)) 
            return this[type].GetString(key, index);        
        return $"type:{type}, key:{key}";
    }

    public string GetString(string type, int key) {
        // MPL_2310_Oткpыт_Зaкpыт$$Paзp. Пycк Ocн. Moтopa
        if (ContainsKey(type))
            return this[type].GetString(key);        
        return $"type:{type}, key:{key}";
    }

    public void AddTranslation(string type, int key, Translation t) {
        if (!ContainsKey(type)) {
            var group = new TranslationGroup();
            group.AddTranslation(key, t);
            Add(type, group);
        }
        else {
            var group = this[type];
            group.AddTranslation(key, t);
        }
    }
}
