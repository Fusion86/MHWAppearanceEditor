import numbers
from typing import Dict, List, Tuple


def main():
    hr_table = parse_xp_table_file("data/HR_EXP_by_level.txt")
    mr_table = parse_xp_table_file("data/MR_EXP_by_level.txt")

    src = xp_table_to_csharp_dict_src(
        [("HunterXpTable", hr_table), ("MasterXpTable", mr_table)]
    )

    with open("assets/XpTable.cs", "w") as f:
        f.write(src)


# This function breaks if you look at it in a funny way.
def parse_xp_table_file(path) -> Dict[int, int]:
    table = {}

    with open(path) as f:
        for line in f.readlines():
            parts = line.split("  ")
            lvl = xp = None
            for part in parts:
                k, v = [x.strip() for x in part.split("=")]
                if k == "Lvl":
                    lvl = int(v)
                else:
                    xp = int(v)

            if lvl != None and xp != None:
                table[lvl] = xp

    return table


def xp_table_to_csharp_dict_src(tables: List[Tuple[str, Dict[int, int]]]) -> str:
    src = ""

    for name, data in tables:
        src += f"public static readonly Dictionary<int, int> {name} = new Dictionary<int, int>\n{{\n"

        for lvl, xp in data.items():
            src += f"    [{lvl}] = {xp},\n"

        src += "};\n\n"

    return src


if __name__ == "__main__":
    main()
