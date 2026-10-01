# Documentation review status

Review date: September 30, 2026. This review separates the guide content, the experience of the published documentation site and product qualification.

## Published reader experience

**User-rated baseline: 4/10.** The published reader has a flat filename-based menu, limited visual hierarchy, no theme selector and no page-specific contribution flow. The earlier 8.0 score described guide content and scoped local previews. It overstated the experience of the live website and is withdrawn as an overall website rating.

The next reader revision adds grouped and nested collapsible navigation, page filtering, a page outline, previous/next links, responsive cards, product branding, light/dark themes and GitHub feedback drafts tied to the actual page and published source. A source commit or passing build does not establish that these features are deployed. **A revised live UX score remains pending hosted verification.**

## Review criteria

| Area | Acceptance evidence required |
| --- | --- |
| Navigation | Every published page reachable once in the selected version; meaningful groups, nested pages, working collapse and filtering |
| Presentation | Clear headings, readable spacing, useful task cards and restrained iconography; diagrams fit their container |
| Branding and themes | Approved product marks; transparent Ink C on light surfaces, White C on dark; persistent Light/Dark/System preference |
| Page orientation | Page title, version context, outline and working previous/next links |
| Accessibility | Keyboard navigation, visible focus, skip link, semantic disclosures, image alternatives and legible contrast in both themes |
| Responsive behavior | Actual narrow-viewport checks; no page overflow; usable navigation, tables and code samples |
| Examples | Accurate request schemas, visible language labels and verified copy behavior |
| Feedback | Valid public GitHub repository and actual source path; draft contains page/version/commit; no automatic submission |
| Content quality | Task instructions, clear limitations, troubleshooting and recovery guidance supported by scoped evidence |
| Performance and cost | Reuse existing infrastructure, bounded assets, no added standing capacity or third-party tracking |

The 11-factor historical content review and its before/after scores remain in the version history and task evidence. They are not a substitute for the reader checks above, customer usability research, a full accessibility audit or product acceptance.

## Validation evidence

- Markdown build: 24 pages, local targets/anchors, one H1, image alternatives and fenced command paths checked with pinned Markdig 0.40.0. Preview styles differ from the hosted portal; hosted navigation/assets require a separate publication check.
- Candidate 2177 owner receipts add six scoped browser workflow rows and 12 native/API policy checks, bound to exact source/image/Windows CLI archive hashes. Native changes were reconciled in fresh browser reads; six fixtures were removed with 404 absence. No runtime helpers were rerun by the documentation task.
- Candidate 2229 later receipts bind exact source `b73e294fa4adb85494445c39056489b53713d6a0`: 19 native/module and five browser CA metadata/material checks passed, followed by 38 role/file/disabled-bridge checks. Candidate 2177's CA metadata-only failure remains historical; active trust, private credential replacement, actual bridge traffic and remote parity remain separate.
- Combined build 2308 passed at exact source `87550462407c80bc01099e7f82dfe14d8c769492`. Build and package-consumer checks do not establish live product acceptance, distributed-runtime activation or a hosted reader rollout.
- Native CLI source build passed. Forty-nine live reads, request schemas and synthetic role/global/role-policy/user/membership mutations passed at Broker `e5b5eb1`.
- MQTT 5 loopback first-message proof passed: allowed-topic publish/observer delivery and denied outside-topic publish/subscribe (`0x87`). This is not complete MQTT conformance.
- Isolated local System events UI save and CLI read-back passed; stale revision rejected; CLI update to revision 2 was visible after UI reload. Provider/bridge editor reads passed; no external tenant/bridge was provisioned.
- Local desktop and 390-pixel preview inspected. Mobile document width did not exceed viewport. Code/tables have bounded scrolling and navigation stays usable.
- External links: OASIS MQTT specification and Azure Portal returned 200. Marketplace offer returned automated 403 and needs entitled-browser verification; it is not called broken or qualified. DEV docs returned 200; PROD product docs returned 404 before this DEV-only change.
- Build and publication receipts are retained with task release evidence, including source SHA and job outcome. A pending job is not counted as successful publication.

## Residual product gaps

- Candidate 2133's missing policy route remains historical. Exact candidate 2177 local browser/direct/native edits passed with read-back, ownership/input rejection and cleanup. Remote/security changes, restart persistence and multi-replica qualification remain open; the later development endpoint guard is excluded from 2177.

- An apparent local editor/API mismatch was traced to concurrent broker cookies sharing localhost across ports; isolated session recheck passed. No product defect is inferred from that initial failure.
- CoreControl live lifecycle/rotation, cross-portal authorized writes, protected upload and multi-replica activation remain separately qualified.
- Current production capacity and ordered-runtime recovery are held; no sizing guarantee comes from 1813 or CI.
- Real Event Hubs/CoreStream, other OIDC tenants, provider-specific restore drills and customer Marketplace paths need exact-release evidence.

The new procedures describe operator checks and recovery planning, not a tested production RPO/RTO or certification. Historical reports keep original outcomes. See [release status](release-status.md).
