'use client'

import { useRouter } from 'next/navigation';
import styles from "@/styles/recipts-page/hero/hero.module.css"

export default function Hero() {
  const router = useRouter();
  const mockArchiveResources = [
    {id:1, resourceName:'ящикf'}
  ]
  return (
    <main className={styles.mainContainer}>
        <h1 className={styles.headerText}>Ресурсы</h1>
        <div className={styles.buttonGroup}>
          <button className={styles.acceptButton} onClick={() => router.push('/resources-page/')}>К рабочим</button>
        </div>
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Наименование</th>
            </tr>
          </thead>
          <tbody>
            {mockArchiveResources.map(archiveResources => (
              <tr key={archiveResources.id} onClick={() => router.push(`/edit-archiveresource/${archiveResources.id}`)}>
                <td>{archiveResources.resourceName}</td>
              </tr>
            ))}
          </tbody>
        </table>
    </main>
  );
}
