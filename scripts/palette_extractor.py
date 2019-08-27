#!/usr/bin/env python3

import os
import json
import config as cfg
from os.path import join
from lib.binreader import BinaryReader


def main():
    source_path = join(cfg.chunks_root, "common/face_edit/face_edit_palette.pal")
    out_file = join(cfg.out_dir, "palette.json")

    os.makedirs(cfg.out_dir, exist_ok=True)

    with open(source_path, "rb") as f:
        br = BinaryReader(f)
        magic = br.read_string(3)

        if magic != "PAL":
            raise Exception("Not a palette file!")

        f.seek(0, 2)
        file_size = f.tell()
        f.seek(8)

        colors = []

        while f.tell() < file_size:
            b = f.read(4)
            color_str = bytes(b).hex().upper()
            colors.append(color_str)

        print("Colors:")
        for color in colors:
            print(color)

        print("\nCount: {}".format(len(colors)))

        with open(out_file, "w") as f_out:
            json.dump(colors, f_out)


if __name__ == "__main__":
    main()
