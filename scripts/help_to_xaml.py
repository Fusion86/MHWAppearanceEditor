"""
Created mostly for fun. There are a few small issues.

- It uses ’ instead of '
- It doesn't support lists
"""


import os
from marko import Markdown, Renderer
from xml.sax.saxutils import escape as _xml_escape


class XamlRenderer(Renderer):
    def render(self, element):
        """Renders the given element to string.
        :param element: a element to be rendered.
        :returns: the output string or any values.
        """
        if not self.root_node:
            self.root_node = element
        render_func = getattr(
            self, self._cls_to_func_name(element.__class__), None)
        if not render_func:
            render_func = self.render_children

        # print(self._cls_to_func_name(element.__class__))
        return render_func(element)

    def render_heading(self, element):
        if element.level == 2:
            fontsize = 18
        elif element.level == 3:
            fontsize = 16
        else:
            fontsize = 14

        return """<TextBlock Text="{children}" FontSize="{fontsize}"/>\n""".format(
            children=self.render_children(element),
            fontsize=fontsize
        )

    def render_blank_line(self, element):
        return """<TextBlock Text=" "/>"""

    def render_paragraph(self, element):
        children = self.render_children(element)
        return """<TextBlock Text="{}"/>\n""".format(children)

    def render_raw_text(self, element):
        return self.xml_escape(element.children)

    def render_line_break(self, element):
        return "&#10;"

    def render_literal(self, element):
        return self.xml_escape(element.children)

    @staticmethod
    def xml_escape(str):
        return _xml_escape(str, entities={
            "'": "&apos;",
            "\"": "&quot;"
        })


if __name__ == "__main__":
    __location__ = os.path.realpath(os.path.join(
        os.getcwd(), os.path.dirname(__file__)))

    # Load help markdown
    with open(os.path.join(__location__, "help.md"), "r", encoding="utf8") as f:
        text = f.read()

    markdown = Markdown(renderer=XamlRenderer)
    print(markdown(text))
