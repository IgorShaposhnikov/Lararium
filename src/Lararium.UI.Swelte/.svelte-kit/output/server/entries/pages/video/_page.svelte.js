import { W as store_get, X as ensure_array_like, Y as unsubscribe_stores } from "../../../chunks/index2.js";
import { g as getContext, e as escape_html } from "../../../chunks/context.js";
import "clsx";
import "@sveltejs/kit/internal";
import "../../../chunks/exports.js";
import "../../../chunks/utils.js";
import "@sveltejs/kit/internal/server";
import "../../../chunks/state.svelte.js";
const getStores = () => {
  const stores$1 = getContext("__svelte__");
  return {
    /** @type {typeof page} */
    page: {
      subscribe: stores$1.page.subscribe
    },
    /** @type {typeof navigating} */
    navigating: {
      subscribe: stores$1.navigating.subscribe
    },
    /** @type {typeof updated} */
    updated: stores$1.updated
  };
};
const page = {
  subscribe(fn) {
    const store = getStores().page;
    return store.subscribe(fn);
  }
};
class Video {
  id = 0;
  name = "no-video";
  duration = "-1";
  quality = "-1";
  size = "-1";
  tags = [];
  thumbnail = "null";
  date = "";
  constructor(data) {
    Object.assign(this, data);
  }
}
function _page($$renderer, $$props) {
  $$renderer.component(($$renderer2) => {
    var $$store_subs;
    let params;
    let currentVideoData = {
      id: 0,
      name: "Поход в горы 2023",
      duration: "35 мин",
      quality: "480p",
      size: "1.2 GB",
      tags: ["Природа", "Путешествие", "Горы", "Отдых"],
      thumbnail: "images/0bf507d9-995b-0dbd-1efd-62aa384d90df.jpg",
      date: "15.06.2023"
    };
    new Video(currentVideoData);
    params = store_get($$store_subs ??= {}, "$page", page).url.searchParams;
    {
      console.log("Page changed:", store_get($$store_subs ??= {}, "$page", page));
    }
    $$renderer2.push(`<div>`);
    {
      $$renderer2.push("<!--[-->");
      $$renderer2.push(`<p>loading...</p> <!--[-->`);
      const each_array = ensure_array_like(params.keys());
      for (let $$index = 0, $$length = each_array.length; $$index < $$length; $$index++) {
        let key = each_array[$$index];
        $$renderer2.push(`<li>${escape_html(key)}</li>`);
      }
      $$renderer2.push(`<!--]-->`);
    }
    $$renderer2.push(`<!--]--></div>`);
    if ($$store_subs) unsubscribe_stores($$store_subs);
  });
}
export {
  _page as default
};
