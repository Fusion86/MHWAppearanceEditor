import subprocess
import config as cfg
from os.path import join


def main():
    tex_path = join(cfg.chunks_root, "Assets/default_tex/skin_BM.tex")
    png_path = join(cfg.out_dir, "skin_color.png")

    args = [cfg.tex_to_png_path, tex_path, png_path]
    subprocess.call(args)


if __name__ == "__main__":
    main()
