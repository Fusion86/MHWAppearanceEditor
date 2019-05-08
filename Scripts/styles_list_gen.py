#!/usr/bin/env python3

"""
This script cuts the spritesheets used in the Monster Hunter World character creation menu in separate .png files.

Requirements:

- TexToPng, see https://github.com/Qowyn/MHWTexToPng/releases
- Extracted Monster Hunter World chunks (all in one folder)
    So not chunk0, chunk1, chunk2, but a merged folder chunks containing all the subfolders.
    This is easily created with MHWNoChunk, see https://www.nexusmods.com/monsterhunterworld/mods/411
"""

import os
import subprocess
from os.path import join
from PIL import Image
from collections import namedtuple


class Config:
    TEXTOPNG_PATH = "L:/Tools/Modding/TexToPng/TexToPng.exe"
    CHUNKS_ROOT = "L:/Sync/MHWMods/chunks"
    OUT = join(os.getcwd(), "styles_list_gen")


Spritesheet = namedtuple("Spritesheet", ["category", "texture_name", "size", "count"])

Items = [
    # Character
    Spritesheet("Male brows", "thumb_brow00_ID", [126, 62], 16),
    Spritesheet("Female brows", "thumb_brow01_ID", [126, 62], 16),
    Spritesheet("Male eyes", "thumb_eye00_ID", [126, 62], 30),
    Spritesheet("Female eyes", "thumb_eye01_ID", [126, 62], 30),
    Spritesheet("Male faces", "thumb_face00_ID", [102, 102], 24),
    Spritesheet("Female faces", "thumb_face01_ID", [102, 102], 24),
    Spritesheet("Male foreheads", "thumb_forehead00_ID", [126, 62], 16),
    Spritesheet("Female foreheads", "thumb_forehead01_ID", [126, 62], 16),
    Spritesheet("Male hairstyles", "thumb_hair00_ID", [102, 102], 28),
    Spritesheet("Female hairstyles", "thumb_hair01_ID", [102, 102], 28),
    Spritesheet("Male inner armors", "thumb_inner00_ID", [102, 102], 4),
    Spritesheet("Female inner armors", "thumb_inner01_ID", [102, 102], 4),
    Spritesheet("Male mouths", "thumb_hair00_ID", [102, 102], 24),
    Spritesheet("Female mouths", "thumb_hair01_ID", [102, 102], 24),
    Spritesheet("Male mustaches", "thumb_mustache00_ID", [102, 102], 21),
    Spritesheet("Female mustaches", "thumb_mustache01_ID", [102, 102], 21),  # heh
    Spritesheet("Male noses", "thumb_nose00_ID", [102, 102], 24),
    Spritesheet("Female noses", "thumb_nose01_ID", [102, 102], 24),
    # Character misc
    Spritesheet("Male facepaints", "thumb_paint00_ID", [102, 102], 34),
    Spritesheet("Female facepaints", "thumb_paint01_ID", [102, 102], 34),
    Spritesheet("Character presets", "thumb_preset_ID", [102, 102], 25),
    # Palico
    Spritesheet("Palico coat types", "thumb_o_coattype_ID", [126, 126], 4),
    Spritesheet("Palico ears", "thumb_o_ear_ID", [126, 126], 5),
    Spritesheet("Palico eyes", "thumb_o_eye_ID", [126, 62], 6),
    Spritesheet("Palico presets", "thumb_o_preset_ID", [126, 126], 12),
    Spritesheet("Palico tails", "thumb_o_tail_ID", [126, 126], 4),
]


if __name__ == "__main__":
    tmpdir = join(Config.OUT, "tmp")
    imgdir = join(Config.OUT, "img")

    # Create needed dirs
    os.makedirs(tmpdir, exist_ok=True)
    os.makedirs(imgdir, exist_ok=True)

    # Where to find needed source files
    chara_make_tex = join(Config.CHUNKS_ROOT, "ui/chara_make/tex")

    for item in Items:
        print(f"\nWorking on '{item.category}'")

        tex_path = join(chara_make_tex, item.texture_name) + ".tex"
        png_path = join(tmpdir, item.texture_name) + ".png"

        print(f"Converting '{tex_path}'")
        print(f"Saving to '{png_path}'")

        # Convert .tex to .png and save in tmpdir
        args = [Config.TEXTOPNG_PATH, tex_path, png_path]
        subprocess.call(args)

        print(f"Cutting file in {item.count} pieces")

        # Load spritesheet which contains all the preview images for this category
        src = Image.open(png_path)
        img_width, _ = src.size
        pieces_per_row = img_width // item.size[0]

        for i in range(item.count):
            dest = join(imgdir, item.texture_name) + f"_{i}.png"

            # Calculate row and start x, y
            row = item.size[0] * (i + 1) // img_width
            start_x = i % pieces_per_row * item.size[0]
            start_y = row * item.size[1]

            box = (start_x, start_y, start_x + item.size[0], start_y + item.size[1])

            img = src.crop(box)
            img.save(dest)

            print(f"Pieces {i+1}/{item.count}", end="\r")  # +1 because zero-indexed

        # Print newline after we are done because we don't do that inside the loop
        print()
