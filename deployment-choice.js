const platforms = {azure:'Azure', aws:'AWS', gcp:'Google Cloud', kubernetes:'Kubernetes', local:'Local / Docker'};
const databases = {postgresql:'PostgreSQL', sqlserver:'SQL Server', cosmos:'Azure Cosmos DB', dynamodb:'DynamoDB', firestore:'Firestore', sqlite:'SQLite'};
const available = {azure:['postgresql','sqlserver','cosmos'], aws:['postgresql','sqlserver','dynamodb'], gcp:['postgresql','sqlserver','firestore'], kubernetes:['postgresql','sqlserver','cosmos'], local:['sqlite']};

export function mount(root, context) {
    let cloud = '', database = '', animation;
    const cloudButtons = [...root.querySelectorAll('[data-cloud]')];
    const databaseButtons = [...root.querySelectorAll('[data-database]')];
    const update = () => {
        if (cloud && database && !available[cloud].includes(database)) database = '';
        cloudButtons.forEach(button => button.setAttribute('aria-pressed', String(button.dataset.cloud === cloud)));
        databaseButtons.forEach(button => {
            button.hidden = !!cloud && !!button.dataset.database && !available[cloud].includes(button.dataset.database);
            button.setAttribute('aria-pressed', String(button.dataset.database === database));
        });
        context.sections.filter({cloud, database});
        root.querySelector('[data-cloud-label]').textContent = platforms[cloud] || 'All platforms';
        root.querySelector('[data-database-label]').textContent = databases[database] || 'All databases';
        root.querySelector('.selection-summary').textContent = cloud || database
            ? `Showing ${platforms[cloud] || 'all platforms'} · ${databases[database] || 'all databases'}. Shared setup guidance remains visible.`
            : 'All deployment documentation is shown.';
        root.host.removeAttribute('data-choice-animation');
        clearTimeout(animation);
        if (!context.motion.matches) {
            requestAnimationFrame(() => { if (!context.signal.aborted) root.host.setAttribute('data-choice-animation','true'); });
            animation = setTimeout(() => root.host.removeAttribute('data-choice-animation'),1100);
        }
    };
    cloudButtons.forEach(button => button.addEventListener('click', () => {
        cloud = button.dataset.cloud; update();
        if (cloud) context.scenes?.goTo('database');
    }, {signal:context.signal}));
    databaseButtons.forEach(button => button.addEventListener('click', () => { database = button.dataset.database; update(); }, {signal:context.signal}));
    root.querySelector('[data-reset]').addEventListener('click', () => { cloud = database = ''; update(); }, {signal:context.signal});
    root.querySelector('[data-open-guide]').addEventListener('click', () => context.sections.focusFirst(), {signal:context.signal});
    context.motion.addEventListener('change', () => { if (context.motion.matches) root.host.removeAttribute('data-choice-animation'); }, {signal:context.signal});
    update();
    return () => { clearTimeout(animation); context.sections.reset(); };
}
