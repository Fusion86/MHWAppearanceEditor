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
import json
import time
import shutil
import subprocess
import config as cfg
from os.path import join
from PIL import Image
from collections import namedtuple


Spritesheet = namedtuple("Spritesheet", ["category", "texture_name", "size", "count", "offset"])

# Commented out items are not needed by MHWAppearanceEditor and therefore also not included (by default)
Items = [
    # Character
    Spritesheet("Male Eyebrows", "thumb_brow00_ID", [126, 62], 16, 0),
    Spritesheet("Female Eyebrows", "thumb_brow01_ID", [126, 62], 16, 0),
    Spritesheet("Male Eyes", "thumb_eye00_ID", [126, 62], 34, 0),
    Spritesheet("Female Eyes", "thumb_eye01_ID", [126, 62], 34, 0),
    Spritesheet("Male Faces", "thumb_face00_ID", [102, 102], 28, 0),
    Spritesheet("Female Faces", "thumb_face01_ID", [102, 102], 28, 0),
    Spritesheet("Male Brow Types", "thumb_forehead00_ID", [126, 62], 24, 0),
    Spritesheet("Female Brow Types", "thumb_forehead01_ID", [126, 62], 24, 0),
    # In the CharacterAppearance struct the hairstyle for females has id `100 + (actual id)`
    # TODO: Hairstyle variants
    # Spritesheet("Male Hairstyles", "thumb_hair00_ID", [102, 102], 31, 0),
    # Spritesheet("Female Hairstyles", "thumb_hair01_ID", [102, 102], 32, 100),
    Spritesheet("Male Clothing", "thumb_inner00_ID", [102, 102], 4, 0),
    Spritesheet("Female Clothing", "thumb_inner01_ID", [102, 102], 4, 0),
    Spritesheet("Male Mouths", "thumb_mouth00_ID", [102, 102], 28, 0),
    Spritesheet("Female Mouths", "thumb_mouth01_ID", [102, 102], 28, 0),
    Spritesheet("Male Facial Hair", "thumb_mustache00_ID", [102, 102], 28, 0),
    Spritesheet("Female Facial Hair", "thumb_mustache01_ID", [102, 102], 28, 0),
    Spritesheet("Male Noses", "thumb_nose00_ID", [102, 102], 28, 0),
    Spritesheet("Female Noses", "thumb_nose01_ID", [102, 102], 28, 0),
    Spritesheet("Male Makeup", "thumb_paint00_ID", [102, 102], 45, 0),
    Spritesheet("Female Makeup", "thumb_paint01_ID", [102, 102], 45, 0),
    # Presets for males and females are stored in the same file. 10 presets per gender + 1 line (5 presets) padding = 25
    # Spritesheet("Character Presets", "thumb_preset_ID", [102, 102], 25, 0),
    # Palico
    # Spritesheet("Palico Outlines", "thumb_o_face_ID", [126, 126], 4, 0),
    Spritesheet("Palico Coat Types", "thumb_o_coattype_ID", [126, 126], 6, 0),
    Spritesheet("Palico Ears", "thumb_o_ear_ID", [126, 126], 12, 0),
    Spritesheet("Palico Eyes", "thumb_o_eye_ID", [126, 62], 12, 0),
    # Spritesheet("Palico Pupils", "thumb_o_pupil_ID", [126, 62], 4, 0),
    # Spritesheet("Palico Presets", "thumb_o_preset_ID", [126, 126], 12, 0),
    Spritesheet("Palico Tails", "thumb_o_tail_ID", [126, 126], 7, 0),
]


def main():
    assetsList = []
    tmpdir = join(cfg.out_dir, "tmp")
    imgdir = join(cfg.out_dir, "character_assets")

    # Create needed dirs
    os.makedirs(tmpdir, exist_ok=True)
    os.makedirs(imgdir, exist_ok=True)

    # Where to find needed source files
    chara_make_tex = join(cfg.chunks_root, "ui/chara_make/tex")

    for item in Items:
        print(f"\nWorking on '{item.category}'")

        tex_path = join(chara_make_tex, item.texture_name) + ".tex"
        png_path = join(tmpdir, item.texture_name) + ".png"

        print(f"Converting '{tex_path}'")
        print(f"Saving to '{png_path}'")

        # Convert .tex to .png and save in tmpdir
        args = [cfg.tex_to_png_path, tex_path, png_path]
        subprocess.call(args)

        print(f"Cutting file in {item.count} pieces")

        # Load spritesheet which contains all the preview images for this category
        src = Image.open(png_path)
        img_width, _ = src.size
        pieces_per_row = img_width // item.size[0]

        assetsList.append({
            "name": item.category,
            "texture_name": item.texture_name,
            "count": item.count,
            "offset": item.offset
        })

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

    with open(join(cfg.out_dir, "character_assets.json"), "w+") as f:
        json.dump({
            "timestamp": int(time.time()),
            "assets": assetsList
        }, f)

    shutil.rmtree(tmpdir)


if __name__ == "__main__":
    main()
