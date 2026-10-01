# Feedback

Use **Feedback** in the broker header to open the dialog. Closing or cancelling returns to the current page without submitting a report.

Enter a title and message. Environment details and a screenshot are optional attachments, each unchecked by default. The **What gets shared with CoreVar?** section explains the submission and contains no selection controls. One consent checkbox authorizes the current report; there is no remembered-permission option. Previous remembered permissions do not bypass this choice.

## Optional screenshot

The broker attempts to capture a rendering of the visible page immediately before opening the dialog. Capture stays in browser memory, and the image is sent only when **Include screenshot** is selected and the report is submitted. Preview it before inclusion. Input fields are hidden in the capture, but other page text can contain private information. Page rendering may differ from a native screenshot, and some content may be unavailable. It does not capture other browser tabs or the desktop.

Screenshots are PNG files limited to 1 MiB. If capture fails or exceeds the limit, feedback remains available without an image. No screenshot is written to persistent browser storage by this feature. Closing the dialog releases the capture reference.

## Sharing

Reports include the feedback text, CoreMQ version, submission time and selected attachments. Optional environment details can include operating system, architecture, cloud vendor and region when known. They do not automatically include MQTT message payloads, credentials or configuration dumps. Review screenshots and feedback text for personal information and secrets.

This submission does not enable background diagnostics or CoreControl connectivity. Availability depends on the deployment configuration and a reachable trusted HTTPS receiver. Screenshot forwarding is covered by simulated-receiver tests; no live feedback report is sent merely to test the dialog.
